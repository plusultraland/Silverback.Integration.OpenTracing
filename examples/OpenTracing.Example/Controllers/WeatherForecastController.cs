using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace OpenTracing.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPublisher _publisher;

        private readonly ITracer _tracer;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPublisher publisher, ITracer tracer)
        {
            _logger = logger;
            _publisher = publisher;
            _tracer = tracer;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var span = _tracer.BuildSpan("get://").StartActive();

            var rng = new Random();
            var values = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            await _publisher.PublishAsync(values);

            return Ok(values);
        }
    }
}
