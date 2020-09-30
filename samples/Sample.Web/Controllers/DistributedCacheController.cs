using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributedCacheController : ControllerBase
    {
        private static readonly Random rng = new Random();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<SimpleCacheController> _logger;

        public DistributedCacheController(
            IDistributedCache distributedCache,
            ILogger<SimpleCacheController> logger
        )
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken)
        {
            // Will get cached response if there is any and fetch otherwise.
            var response = await _distributedCache
                .GetOrSetJsonObjectAsync("WeatherForecastController_Get",
                    () => FetchAllForecastsAsync(cancellationToken),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5) },
                    cancellationToken);

            return response;
        }

        private async Task<IEnumerable<WeatherForecast>> FetchAllForecastsAsync(CancellationToken cancellationToken)
        {
            var forecasts = new List<WeatherForecast>();

            for (var index = 1; index < 5; index++)
            {
                var date = DateTime.Now.Date.AddDays(index);

                // Get cached daily forecast if it exists and fetch if not.
                var forecast = await _distributedCache
                    .GetOrSetJsonObjectAsync($"single-weather-forecast:{date.ToShortDateString()}",
                        () => FetchSingleForecastAsync(date, cancellationToken),
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15) },
                        cancellationToken);

                forecasts.Add(forecast);
            }

            return forecasts.AsEnumerable();
        }

        private async Task<WeatherForecast> FetchSingleForecastAsync(DateTime date, CancellationToken cancellationToken)
        {
            // Simulate access to a database or third party service.
            await Task.Delay(100, cancellationToken);

            // Return mock result.
            return new WeatherForecast
            {
                Date = date,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }
    }
}
