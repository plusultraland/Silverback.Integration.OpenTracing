using Silverback.Integration.OpenTracing;
using Silverback.Messaging.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SilverbackBuilderExtensions
    {
        public static ISilverbackBuilder UseOpenTracing(this ISilverbackBuilder builder)
        {
            builder.Services
                .AddSingletonBrokerBehavior<ConsumerTracingBehavior>()
                .AddSingletonBrokerBehavior<ProducerTracingBehavior>();

            return builder;
        }
    }
}