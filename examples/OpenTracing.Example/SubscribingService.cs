using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Subscribers;

namespace OpenTracing.Example
{
    public class SubscribingService : ISubscriber
    {
        private readonly ILogger<SubscribingService> _logger;

        public SubscribingService(ILogger<SubscribingService> logger)
        {
            _logger = logger;
        }

        public Task OnMessageReceived(IInboundEnvelope<WeatherForecast> message)
        {
            // ...your message handling loging...
            using (_logger.BeginScope($"Consuming message {message.Message.Summary}"))
            {
                _logger.LogInformation("completed");
            }

            return Task.CompletedTask;
        }
    }
}