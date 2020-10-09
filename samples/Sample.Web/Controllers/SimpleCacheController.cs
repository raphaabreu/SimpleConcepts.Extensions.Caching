using System.Collections.Generic;
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
        private readonly ISimpleWeatherService _simpleWeatherService;

        public SimpleCacheController(
            ISimpleCache<IEnumerable<WeatherForecast>> responseCache,
            ISimpleWeatherService simpleWeatherService
        )
        {
            _responseCache = responseCache;
            _simpleWeatherService = simpleWeatherService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken)
        {
            // Will get cached response if there is any and fetch otherwise.
            var response = await _responseCache
                .GetOrSetAsync(() => _simpleWeatherService.FetchAsync(cancellationToken), cancellationToken);

            return response;
        }
    }
}
