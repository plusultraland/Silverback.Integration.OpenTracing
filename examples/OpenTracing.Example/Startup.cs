using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silverback.Messaging;
using Silverback.Messaging.Broker;
using Silverback.Messaging.Configuration;

namespace OpenTracing.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddOpenTracing(Configuration);

            services.AddSilverback()
                    .UseOpenTracing()
                    .WithConnectionToMessageBroker(b =>{
                        b.AddInMemoryBroker();
                        b.AddInboundConnector();
                        b.AddOutboundConnector();
                    })
                    .AddScopedSubscriber<SubscribingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime, BusConfigurator busConfigurator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Ensure ITracer is initalized
            app.ApplicationServices.GetRequiredService<ITracer>();

            var broker = busConfigurator.Connect(endpoitns =>
                                endpoitns
                                    .AddInbound(new KafkaConsumerEndpoint("teste"))
                                    .AddOutbound<WeatherForecast>(new KafkaProducerEndpoint("teste")));
            
            appLifetime.ApplicationStopping.Register(() => broker.Disconnect());
        }
    }
}
