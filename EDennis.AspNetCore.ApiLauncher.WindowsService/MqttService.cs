using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.ApiLauncher {

    /// <summary>
    /// This class provides an MQTT server -- a broker that
    /// publishes messages to itself and other clients, and
    /// listens for incoming messages from clients.  The
    /// server is used to allow clients to send and receive
    /// messages concerning which APIs are needed (incoming
    /// messages) and which APIs are launched (outgoing messages)
    /// </summary>
    public class MqttService : IHostedService, IDisposable {

        //references to injected objects
        private readonly ILogger _logger;
        private readonly IOptions<MqttConfig> _config;
        private readonly Launcher _launcher;

        //the underlying MQTT server.
        private IMqttServer _mqttServer;

        //incoming message topic
        private const string SERVER_ACCEPT_TOPIC = "NeedApis";

        //outgoing message topic
        private const string SERVER_PUBLISH_TOPIC = "HaveApis";

        /// <summary>
        /// Constructs a new MqttService object
        /// </summary>
        /// <param name="logger">ILogger instance</param>
        /// <param name="config">Configuration singleton</param>
        /// <param name="launcher">Launcher singleton</param>
        public MqttService(ILogger<MqttService> logger, IOptions<MqttConfig> config,
            Launcher launcher) {
            _logger = logger;
            _config = config;
            _launcher = launcher;
        }

        /// <summary>
        /// Starts the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken) {

            _logger.LogInformation("Starting MQTT Daemon on port " + _config.Value.Port);

            //Building the config
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(1000)
                .WithDefaultEndpointPort(_config.Value.Port);

            //instantiate a new MQTT server
            _mqttServer = new MqttFactory().CreateMqttServer();

            //wire up the events
            _mqttServer.ClientSubscribedTopic += _mqttServer_ClientSubscribedTopic;
            _mqttServer.ClientUnsubscribedTopic += _mqttServer_ClientUnsubscribedTopic;
            _mqttServer.ClientConnected += _mqttServer_ClientConnected;
            _mqttServer.ClientDisconnected += _mqttServer_ClientDisconnected;
            _mqttServer.ApplicationMessageReceived += _mqttServer_ApplicationMessageReceived;

            //start the server
            await _mqttServer.StartAsync(optionsBuilder.Build());

            return;
        }

        /// <summary>
        /// Event handler for received application messages.  
        /// When a NeedsApi/ClientId message is sent to this server,
        /// it ensures that all APIs are launched, and it returns
        /// the port assignments (and other data) to the requesting
        /// client.
        /// </summary>
        /// <param name="sender">the object responsible for the event</param>
        /// <param name="e">the event arguments</param>
        private void _mqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e) {

            _logger.LogInformation($"{e.ClientId} sent message with topic {e.ApplicationMessage.Topic}");

            //get the topic and clientId
            var topic = e.ApplicationMessage.Topic;
            var topicTokens = topic.Split("/");
            var topicString = topicTokens[0];
            var clientId = e.ClientId; //also, topicTokens[1];

            //get the message body
            var payloadBytes = e.ApplicationMessage.Payload;
            var payloadString = Encoding.UTF8.GetString(payloadBytes, 0, payloadBytes.Length);
            _logger.LogDebug($"Message Payload\n{payloadString}");

            //if this is a server-initiated message, stop processing it
            if (!topic.StartsWith(SERVER_ACCEPT_TOPIC)) {
                return;
            }

            //initialize an object for holding the needed APIs
            Dictionary<string, Api> needApis;

            try {
                //deserialize the needed APIs
                needApis = JToken.Parse(payloadString).ToObject<Dictionary<string,Api>>();
            } catch (Exception ex) {
                _logger.LogError($"EXCEPTION with message\n:{payloadString}\n{ex.Message}");
                throw new ArgumentException(ex.Message);
            }

            //if an empty object is passed it, stop all of the client's needed APIs
            if (needApis.Count == 0) {
                _launcher.StopApis(clientId);
            } else {
                //otherwise, start all of the APIs
                //NOTE: this process will block until all APIs are fully
                //configured
                _launcher.StartApis(needApis,clientId);

                //serialize the dictionary of launched APIs to a JSON string
                var haveApisJson = JToken.FromObject(_launcher.LaunchedApis).ToString();

                //build the response message
                var msg = new MqttApplicationMessageBuilder()
                    .WithExactlyOnceQoS()
                    .WithTopic(SERVER_PUBLISH_TOPIC + "/" + clientId)
                    .WithPayload(haveApisJson)
                    .Build();

                //send the message
                _mqttServer.PublishAsync(msg);

                foreach (var api in _launcher.LaunchedApis)
                    _logger.LogInformation($"Server has {api.Value.ProjectName} @ {api.Value.Port}");
            }


        }

        /// <summary>
        /// When a client is disconnected from the server, stop all of
        /// its needed APIs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e) {
            _logger.LogInformation(e.ClientId + " Disconnected.  Stopping Associated APIs");
            _launcher.StopApis(e.ClientId);
        }

        /// <summary>
        /// Log when a client is connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e) {
            _logger.LogInformation(e.ClientId + " Connected.");
        }

        /// <summary>
        /// Log when a client unsubscribes to a topic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mqttServer_ClientUnsubscribedTopic(object sender, MqttClientUnsubscribedTopicEventArgs e) {
            _logger.LogInformation(e.ClientId + " unsubscribed to " + e.TopicFilter);
        }

        /// <summary>
        /// Log when a client subscribes to a topic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mqttServer_ClientSubscribedTopic(object sender, MqttClientSubscribedTopicEventArgs e) {
            _logger.LogInformation(e.ClientId + " subscribed to " + e.TopicFilter);
        }

        /// <summary>
        /// When this Service is shutdown, ensure that all running 
        /// APIs are shutdown also.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("Stopping Mqtt Daemon.");
            if(_launcher.LaunchedApis.Count > 0)
                _launcher.StopApis();
            return _mqttServer.StopAsync();
        }

        public void Dispose() {
            _logger.LogInformation("Disposing....");
            _launcher.Dispose();
        }
    }
}