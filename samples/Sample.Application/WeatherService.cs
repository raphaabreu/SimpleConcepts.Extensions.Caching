using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application
{
    public class WeatherService : IWeatherService
    {
        private static readonly Random RNG = new Random();
        private static readonly string[] SUMMARIES = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast> FetchForecastAsync(DateTime date, CancellationToken cancellationToken)
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