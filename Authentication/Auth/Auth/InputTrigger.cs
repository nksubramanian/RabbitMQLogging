using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace Auth
{
    public class InputTrigger
    {
        private readonly ILogger _logger;

        public InputTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<InputTrigger>();
        }

        [Function("InputTrigger")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
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
            channel.QueueDeclare("myqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var message = new { Name = "Producer", Message = "Hello!" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", "myqueue", null, body);

            return response;
        }
    }
}
