using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenTracingConfiguration
    {
        public static IServiceCollection AddOpenTracing(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var config = Jaeger.Configuration.FromIConfiguration(loggerFactory, configuration);

                var tracer = config.GetTracer();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });

            return services;
        }
    }
}