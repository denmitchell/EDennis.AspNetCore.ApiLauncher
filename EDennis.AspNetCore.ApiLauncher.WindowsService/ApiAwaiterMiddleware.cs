using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.ApiLauncher.WindowsService {

    /// <summary>
    /// This middleware is designed to make sure that all APIs
    /// are available before a request is allowed to be processed
    /// through the rest of the pipeline.  Note: this should be
    /// used for testing purposes only.
    /// </summary>
    public class ApiAwaiterMiddleware {
        private readonly RequestDelegate _next;
        public const int MAX_TIMEOUT_IN_SECONDS = 30;


        public ApiAwaiterMiddleware(RequestDelegate next) {
            _next = next;
        }

        /// <summary>
        /// This method is the method entry point for the middleware.
        /// In this case, the method waits for the "Apis" section of
        /// the configuration object to be updated with BaseAddress
        /// information
        /// </summary>
        /// <param name="context">the HttpContext</param>
        /// <param name="configuration">The configuration singleton</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IConfiguration configuration) {

            var seconds = MAX_TIMEOUT_IN_SECONDS;

            //instantiate an Api dictionary to bind to
            var apis = new Dictionary<string, Api>();

            //loop until all Apis are ready or the timeout period has expired
            while (seconds > 0) {
                //bind to the Api dictionary
                configuration.GetSection("Apis").Bind(apis);

                //get the count of Apis, as well as the count of records with non-null BaseAddress and non-true Ready 
                var apiCount = apis.Count();
                var baseAddressCount = apis.Where(x => x.Value.BaseAddress != null).Count();
                var notReadyCount = apis.Where(x => x.Value.Ready == null || !x.Value.Ready.Value).Count();

                //throw exception if no Apis were defined
                if (apiCount == 0)
                    throw new InvalidOperationException("Cannot use ApiAwaiter middleware without an Apis section");
                //if all records have a BaseAddress and are Ready to be used, break the loop
                else if (baseAddressCount == apiCount && notReadyCount == 0)
                    break;
                Thread.Sleep(1000); //otherwise, sleep for a second
                seconds--; //decrement countdown
            };


            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }

    /// <summary>
    /// This class provides an extension method to IApplicationBuilder
    /// that allows one to add the ApiAwaiterMiddleware to the request
    /// pipeline.
    /// </summary>
    public static class IApplicationBuilderExtensions_ApiAwaiterMiddleware {
        /// <summary>
        /// Adds the ApiAwaiterMiddleware to the request pipeline
        /// </summary>
        /// <param name="builder">IApplicationBuilder singleton</param>
        /// <returns>the singleton (for fluent construction)</returns>
        public static IApplicationBuilder UseApiAwaiter(this IApplicationBuilder builder) {
            builder.UseMiddleware<ApiAwaiterMiddleware>();
            return builder;
        }
    }

}
