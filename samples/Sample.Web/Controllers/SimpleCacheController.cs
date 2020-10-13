using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Application;
using SimpleConcepts.Extensions.Caching;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleCacheController : ControllerBase
    {
        private readonly ISimpleCache<IEnumerable<WeatherForecast>> _responseCache;
        private readonly ISimpleCache<DateTime, WeatherForecast> _weatherForecastCache;

        public SimpleCacheController(
            ISimpleCache<IEnumerable<WeatherForecast>> responseCache,
            ISimpleCache<DateTime, WeatherForecast> weatherForecastCache
        )
        {
            // In this example, responseCache will use extension methods to get or set a value and
            // weatherForecastCache will use the configured value factory to provide values if none exist in cache
            _responseCache = responseCache;
            _weatherForecastCache = weatherForecastCache;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken)
        {
            // Will get cached response if there is any and fetch otherwise.
            var response = await _responseCache
                .GetOrSetAsync(async () =>
                {
                    var tasks = Enumerable
                        .Range(1, 5)
                        .Select(i => _weatherForecastCache.GetAsync(DateTime.Now.Date.AddDays(i), cancellationToken))
                        .ToArray();

                    var forecasts = await Task.WhenAll(tasks);

                    return forecasts.AsEnumerable();
                }, cancellationToken);

            return response;
        }
    }
}
