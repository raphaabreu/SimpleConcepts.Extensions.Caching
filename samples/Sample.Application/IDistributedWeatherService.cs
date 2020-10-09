using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application
{
    public interface IDistributedWeatherService
    {
        Task<IEnumerable<WeatherForecast>> FetchAsync(CancellationToken cancellationToken);
    }
}