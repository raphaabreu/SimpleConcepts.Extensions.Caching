using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Sample.Application
{
    public class DistributedWeatherService : IDistributedWeatherService
    {
        private static readonly Random RNG = new Random();
        private static readonly string[] SUMMARIES = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedCache _distributedCache;

        private readonly DistributedCacheEntryOptions _entryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
        };

        public DistributedWeatherService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<WeatherForecast>> FetchAsync(CancellationToken cancellationToken)
        {
            var forecasts = new List<WeatherForecast>();

            for (var index = 1; index < 5; index++)
            {
                var date = DateTime.Now.Date.AddDays(index);

                // Get cached daily forecast if it exists and fetch if not.
                var forecast = await _distributedCache
                    .GetOrSetJsonObjectAsync($"single-weather-forecast:{date.ToShortDateString()}",
                        () => FetchSingleAsync(date, cancellationToken),
                        _entryOptions,
                        cancellationToken);

                forecasts.Add(forecast);
            }

            return forecasts.AsEnumerable();
        }

        private async Task<WeatherForecast> FetchSingleAsync(DateTime date, CancellationToken cancellationToken)
        {
            // Simulate access to a database or third party service.
            await Task.Delay(100, cancellationToken);

            // Return mock result.
            return new WeatherForecast
            {
                Date = date,
                TemperatureC = RNG.Next(-20, 55),
                Summary = SUMMARIES[RNG.Next(SUMMARIES.Length)]
            };
        }
    }
}