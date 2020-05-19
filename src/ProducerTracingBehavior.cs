using System.Threading.Tasks;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;
using Silverback.Messaging.Broker.Behaviors;

namespace Silverback.Integration.OpenTracing
{
    public class ProducerTracingBehavior : IProducerBehavior
    {
        public async Task Handle(ProducerPipelineContext context, ProducerBehaviorHandler next)
        {
            var envelope = context.Envelope;

            var spanBuilder = GlobalTracer.Instance.BuildSpan($"Publish message on topic {envelope.Endpoint.Name}")
                   .WithTag(Tags.SpanKind, Tags.SpanKindProducer)
                   .WithTag("endpoint", envelope.Endpoint.Name);

            using (var scope = spanBuilder.StartActive())
            {
                GlobalTracer.Instance.Inject(
                   GlobalTracer.Instance.ActiveSpan.Context,
                   BuiltinFormats.TextMap,
                   new SilverbackTextMapInjectAdapter(envelope));

                await next(context);
            }
        }
    }
}