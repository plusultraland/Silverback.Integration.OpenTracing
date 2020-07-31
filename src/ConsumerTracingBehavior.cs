using System;
using System.Linq;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;
using Silverback.Messaging.Broker.Behaviors;

namespace Silverback.Integration.OpenTracing
{
    public class ConsumerTracingBehavior : IConsumerBehavior
    {
        public async Task Handle(ConsumerPipelineContext context, IServiceProvider serviceProvider, ConsumerBehaviorHandler next)
        {
            var envelope = context.Envelopes.Single();

            var operationName = $"Consuming Message on topic {envelope.Endpoint.Name}";

            ISpanBuilder spanBuilder;

            try
            {
                var headers = envelope.Headers.ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
                var parentSpanCtx = GlobalTracer.Instance.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(headers));

                spanBuilder = GlobalTracer.Instance.BuildSpan(operationName);

                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch
            {
                spanBuilder = GlobalTracer.Instance.BuildSpan(operationName);
            }

            spanBuilder
                .WithTag(Tags.SpanKind, Tags.SpanKindConsumer)
                .WithTag("endpoint", envelope.Endpoint.Name);

            using (spanBuilder.StartActive())
            {
                await next(context, serviceProvider);
            }
        }
    }
}