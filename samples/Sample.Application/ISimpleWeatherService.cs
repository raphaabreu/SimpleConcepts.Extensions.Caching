using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application
{
    public interface ISimpleWeatherService
    {
        Task<IEnumerable<WeatherForecast>> FetchAsync(CancellationToken cancellationToken);
    }
}