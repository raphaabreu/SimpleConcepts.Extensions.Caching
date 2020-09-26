using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleConcepts.Extensions.Caching;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ISimpleCache<Guid, IEnumerable<WeatherForecast>> _cache;
        private readonly ISimpleCacheFactory _simpleCacheFactory;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ISimpleCache<Guid, IEnumerable<WeatherForecast>> cache,
            ISimpleCacheFactory simpleCacheFactory,
            ILogger<WeatherForecastController> logger
        )
        {
            _cache = cache;
            _simpleCacheFactory = simpleCacheFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var data = _cache.GetAsync()

            var cache2 = _simpleCacheFactory.Create<string, WeatherForecast>();


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
