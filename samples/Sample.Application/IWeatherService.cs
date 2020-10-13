using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application
{
    public interface IWeatherService
    {
        Task<WeatherForecast> FetchForecastAsync(DateTime date, CancellationToken cancellationToken);
    }
}