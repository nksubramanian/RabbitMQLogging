using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentFunctions
{
    internal class SimpleMiddleware :IFunctionsWorkerMiddleware
    {
        public ITelemetryInitializer _telemetryInitializer;

        public SimpleMiddleware(ITelemetryInitializer telemetryInitializer)
        {
            
            _telemetryInitializer = telemetryInitializer;
            
        }



        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var t = _telemetryInitializer.GetHashCode();
            //TODO: logic goes here.
            return next(context);

        }


    }
}
