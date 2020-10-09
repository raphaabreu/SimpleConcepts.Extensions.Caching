using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Sample.Application;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributedCacheController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IDistributedWeatherService _distributedWeatherService;

        public DistributedCacheController(
            IDistributedCache distributedCache,
            IDistributedWeatherService distributedWeatherService
        )
        {
            _distributedCache = distributedCache;
            _distributedWeatherService = distributedWeatherService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken)
        {
            // Will get cached response if there is any and fetch otherwise.
            var response = await _distributedCache
                .GetOrSetJsonObjectAsync("WeatherForecastController_Get",
                    () => _distributedWeatherService.FetchAsync(cancellationToken),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5) },
                    cancellationToken);

            return response;
        }
    }
}
