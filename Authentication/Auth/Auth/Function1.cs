using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Auth
{
    public static class Function1
    {
        private static ActivitySource source = new ActivitySource("Sample.DistributedTracing", "1.0.0");
        [Function("Function1")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("subramanian logging");

            
            using (Activity activity = source.StartActivity("SomeWork"))
            {
                var contextToInject = Activity.Current.Context;

            }


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functionsxxx!");

            return response;
        }
    }
}
