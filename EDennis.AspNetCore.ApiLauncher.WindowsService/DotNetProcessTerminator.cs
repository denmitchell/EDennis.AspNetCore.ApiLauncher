﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.ApiLauncher {

    /// <summary>
    /// Kills all dotnet processes in order to free up
    /// ports/resources.  This class is used as a fail-safe
    /// means of ensuring that ports are released when they
    /// are no longer needed by the process that is using them.
    /// </summary>
    public class DotNetProcessTerminator {

        private readonly ILogger _logger;

        /// <summary>
        /// Instantiates a new DotNetProcessTerminator objet
        /// with the provided ILogger
        /// </summary>
        /// <param name="logger"></param>
        public DotNetProcessTerminator(ILogger<DotNetProcessTerminator> logger) {
            _logger = logger;
        }


        /// <summary>
        /// Kills dotnet processes running on the current machine,
        /// limited to those attached to particular ports, as well
        /// as their child processes.
        /// </summary>
        /// <param name="ports">the ports to kill</param>
        /// <returns></returns>
        public List<PortProcess> KillDotNetProcesses(int[] ports) {
            //get all dotnet.exe processes and their associated ports
            var portProcesses = GetDotNetPortProcesses();
            //limit the list to those that are associated with one of the designated ports
            var targetProcesses = portProcesses.Where(p => ports.Contains(p.PortNumber));

            //iterate over all target processes and kill them and their child processes
            foreach (var targetProcess in targetProcesses) {
                _logger.LogInformation($"Killing {targetProcess.ProcessId} using port {targetProcess.PortNumber}");
                KillProcessAndChildren(targetProcess.ProcessId);
            }

            //return those process that were killed
            return portProcesses;
        }

        /// <summary>
        /// Gets all dotnet processes with port information
        /// Adapted slightly from https://gist.github.com/cheynewallace/5971686
        /// </summary>
        /// <returns>list of processes/ports</returns>
        public List<PortProcess> GetDotNetPortProcesses() {

            //initialize a list
            var portProcesses = new List<PortProcess>();

            try {
                //use netstat.exe to get process/port info
                using (Process p = new Process()) {

                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.Arguments = "-a -n -o";
                    ps.FileName = "netstat.exe";
                    ps.CreateNoWindow = true;
                    ps.RedirectStandardError = true;
                    ps.RedirectStandardInput = true;
                    ps.RedirectStandardOutput = true;
                    ps.UseShellExecute = false;

                    p.StartInfo = ps;
                    p.Start();

                    StreamReader stdOutput = p.StandardOutput;
                    StreamReader stdError = p.StandardError;

                    string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    string exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0") {
                        // Command Errored. Handle Here If Need Be
                    }

                    //get the rows
                    string[] rows = Regex.Split(content, "\r\n");
                    foreach (string row in rows) {

                        //split the row into tokens
                        string[] tokens = Regex.Split(row, "\\s+");
                        if (tokens.Length > 4 && tokens[1].Equals("TCP")) {
                            string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");

                            //add the new PortProcess to the list
                            portProcesses.Add(new PortProcess {
                                ProcessId = short.Parse(tokens[5]),
                                PortNumber = int.Parse(localAddress.Split(':')[1]),
                                ProcessName = LookupProcess(short.Parse(tokens[5]))
                            });
                        }
                    }
                }
            } catch (Exception ex) {
                _logger.LogError($"Exception {ex.Message}");
            }

            //limit the list by dotnet processes
            return portProcesses.Where(p => p.ProcessName == "dotnet").ToList();
        }

        /// <summary>
        /// Looks up a process name by process id
        /// </summary>
        /// <param name="pid">the process id</param>
        /// <returns>the process name</returns>
        private static string LookupProcess(int pid) {
            string procName;
            try { procName = Process.GetProcessById(pid).ProcessName; } catch (Exception) { procName = "-"; }
            return procName;
        }

        // <summary>
        /// Kill a process, and all of its children, grandchildren, etc.
        /// From https://stackoverflow.com/a/10402906.  Note that this
        /// method is recursive; so, it is best used sparingly.
        /// 
        /// Dependencies: System.Management
        /// </summary>
        /// <param name="pid">Process ID.</param>
        public static void KillProcessAndChildren(int pid) {
            // Cannot close 'system idle process'.
            if (pid == 0) {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc) {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            } catch (ArgumentException) {
                // Process already exited.
            }
        }

        /// <summary>
        /// This class holds port/process information
        /// </summary>
        public class PortProcess {
            public int ProcessId { get; set; }
            public int PortNumber { get; set; }
            public string ProcessName { get; set; }
        }

    }
}
