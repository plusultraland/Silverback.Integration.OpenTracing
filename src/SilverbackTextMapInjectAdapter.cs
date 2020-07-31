using System;
using System.Collections;
using System.Collections.Generic;
using OpenTracing;
using OpenTracing.Propagation;
using Silverback.Messaging.Messages;

namespace Silverback.Integration.OpenTracing
{
    public class SilverbackTextMapInjectAdapter : ITextMap
    {
        private readonly IOutboundEnvelope _envelope;

        public SilverbackTextMapInjectAdapter(IOutboundEnvelope envelope)
        {
            _envelope = envelope;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotSupportedException(
                $"{nameof(TextMapInjectAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Inject)}");
        }

        public void Set(string key, string value)
        {
            _envelope.Headers.AddOrReplace(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}