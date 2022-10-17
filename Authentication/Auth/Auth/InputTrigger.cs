using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace Auth
{
    public class InputTrigger
    {
        private readonly ILogger _logger;

        private static ActivitySource source = new ActivitySource("Sample.DistributedTracing", "1.0.0");
        public InputTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<InputTrigger>();
        }

        [Function("InputTrigger")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            var x = context.GetHashCode();
            _logger.LogInformation("C# HTTP trigger function processed a request");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://gmtxfped:q9q2CrgirKMatFilFe-qlM4cMzGa4hii@beaver.rmq.cloudamqp.com/gmtxfped")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var basicProps = channel.CreateBasicProperties();
            channel.QueueDeclare("myqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var message = new { Name = "Producer", Message = "Hello!" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            //httpClient.DefaultRequestHeaders.Add("traceparent", _mxAContextAccessor.FunctionContext.TraceContext.TraceParent);

            TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;

           

            using (Activity activity = source.StartActivity("SomeWork"))
            {
                var contextToInject = Activity.Current.Context;
            }


            channel.BasicPublish("", "myqueue", null, body);

            return response;
        }
    }
}
