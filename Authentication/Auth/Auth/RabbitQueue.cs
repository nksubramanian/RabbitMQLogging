using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Auth
{
    public class RabbitQueue
    {
        private readonly ILogger _logger;

        public RabbitQueue(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RabbitQueue>();
        }

        [Function("RabbitQueue")]
        public void Run([RabbitMQTrigger("myqueue", ConnectionStringSetting = "CONNECTION_STRING")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
