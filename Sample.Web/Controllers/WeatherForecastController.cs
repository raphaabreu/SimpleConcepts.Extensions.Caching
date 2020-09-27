using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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

        private readonly ISimpleCache<IEnumerable<WeatherForecast>> _cache;
        private readonly ISimpleCacheFactory _simpleCacheFactory;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ISimpleCache<IEnumerable<WeatherForecast>> cache,
            ISimpleCacheFactory simpleCacheFactory,
            IDistributedCache distributedCache,
            ILogger<WeatherForecastController> logger
        )
        {
            _cache = cache;
            _simpleCacheFactory = simpleCacheFactory;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _distributedCache.SetString("test1", "blabla");
            _distributedCache.SetJsonObject("test-key", new WeatherForecast());
            await _distributedCache.SetJsonObjectAsync("test-key", new WeatherForecast());

            var x = _distributedCache.GetJsonObject<WeatherForecast>("test-key");


            var data = _cache.GetAsync();

            var cache2 = _simpleCacheFactory.Create<string, WeatherForecast>("custom-1");

            var res = await cache2.GetAsync("teste123", async () =>
            {
                await Task.Delay(1000);

                return new WeatherForecast();
            });

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
