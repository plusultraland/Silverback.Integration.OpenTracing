using System.Threading.Tasks;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;
using Silverback.Messaging.Broker;
using Silverback.Messaging.Messages;

namespace Silverback.Integration.OpenTracing
{
    public class ProducerTracingBehavior : IProducerBehavior
    {
        public async Task Handle(RawBrokerEnvelope envelope, RawBrokerMessageHandler next)
        {
            var spanBuilder = GlobalTracer.Instance.BuildSpan($"Publish message {envelope.Endpoint.Name}")
                   .WithTag(Tags.SpanKind, Tags.SpanKindProducer)
                   .WithTag("endpoint", envelope.Endpoint.Name);

            using (var scope = spanBuilder.StartActive())
            {
                GlobalTracer.Instance.Inject(
                   GlobalTracer.Instance.ActiveSpan.Context,
                   BuiltinFormats.TextMap,
                   new SilverbackTextMapInjectAdapter(envelope));

                await next(envelope);
            }
        }
    }
}