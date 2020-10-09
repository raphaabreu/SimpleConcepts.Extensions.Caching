using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleConcepts.Extensions.Caching;

namespace Sample.Application
{
    public class SimpleWeatherService : ISimpleWeatherService
    {
        private static readonly Random RNG = new Random();
        private static readonly string[] SUMMARIES = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ISimpleCache<DateTime, WeatherForecast> _dailyForecastCache;

        public SimpleWeatherService(ISimpleCache<DateTime, WeatherForecast> dailyForecastCache)
        {
            _dailyForecastCache = dailyForecastCache;
        }

        public async Task<IEnumerable<WeatherForecast>> FetchAsync(CancellationToken cancellationToken)
        {
            var forecasts = new List<WeatherForecast>();

            for (var index = 1; index < 5; index++)
            {
                var date = DateTime.Now.Date.AddDays(index);

                // Get cached daily forecast if it exists and fetch if not.
                var forecast = await _dailyForecastCache
                    .GetOrSetAsync(date, () => FetchSingleAsync(date, cancellationToken), cancellationToken);

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
