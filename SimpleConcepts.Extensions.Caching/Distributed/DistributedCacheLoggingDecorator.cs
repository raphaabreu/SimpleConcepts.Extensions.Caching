using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public class DistributedCacheLoggingDecorator : IDistributedCache
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<IDistributedCache> _logger;

        public DistributedCacheLoggingDecorator(IDistributedCache cache, ILogger<IDistributedCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public byte[]? Get(string key)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Loading key {CacheKey}", key);

                stopwatch.Start();
                var result = _cache.Get(key);
                stopwatch.Stop();

                _logger.LogInformation("Loaded {CacheValueByteCount} bytes for key {CacheKey} in {ElapsedMilliseconds}ms", result?.Length ?? -1, key, stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load key {CacheKey}", key);
                throw;
            }
        }

        public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Loading key {CacheKey}", key);

                stopwatch.Start();
                var result = await _cache.GetAsync(key, token);
                stopwatch.Stop();

                _logger.LogInformation("Loaded {CacheValueByteCount} bytes for key {CacheKey} in {ElapsedMilliseconds}ms", result?.Length ?? -1, key, stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load value for key {CacheKey}", key);
                throw;
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Saving {CacheValueByteCount} bytes for key {CacheKey}", value?.Length ?? -1, key);

                stopwatch.Start();
                _cache.SetAsync(key, value, options);
                stopwatch.Stop();

                _logger.LogInformation("Saved {CacheValueByteCount} bytes for key {CacheKey} in {ElapsedMilliseconds}", value?.Length ?? -1, key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to save {CacheValueByteCount} bytes for key {CacheKey}", value?.Length ?? -1, key);
                throw;
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Saving {CacheValueByteCount} bytes for key {CacheKey}", value?.Length ?? -1, key);

                stopwatch.Start();
                await _cache.SetAsync(key, value, options, token);
                stopwatch.Stop();

                _logger.LogInformation("Saved {CacheValueByteCount} bytes for key {CacheKey} in {ElapsedMilliseconds}", value?.Length ?? -1, key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to save {CacheValueByteCount} bytes for key {CacheKey}", value?.Length ?? -1, key);
                throw;
            }
        }

        public void Refresh(string key)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Refreshing key {CacheKey}", key);

                stopwatch.Start();
                _cache.Refresh(key);
                stopwatch.Stop();

                _logger.LogInformation("Refreshed key {CacheKey} in {ElapsedMilliseconds}", key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to refresh key {CacheKey}", key);
                throw;
            }
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Refreshing key {CacheKey}", key);

                stopwatch.Start();
                await _cache.RefreshAsync(key, token);
                stopwatch.Stop();

                _logger.LogInformation("Refreshed key {CacheKey} in {ElapsedMilliseconds}", key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to refresh key {CacheKey}", key);
                throw;
            }
        }

        public void Remove(string key)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Removing key {CacheKey}", key);

                stopwatch.Start();
                _cache.Remove(key);
                stopwatch.Stop();

                _logger.LogInformation("Removed key {CacheKey} in {ElapsedMilliseconds}", key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to remove key {CacheKey}", key);
                throw;
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            var stopwatch = new Stopwatch();
            try
            {
                _logger.LogDebug("Removing key {CacheKey}", key);

                stopwatch.Start();
                await _cache.RemoveAsync(key, token);
                stopwatch.Stop();

                _logger.LogInformation("Removed key {CacheKey} in {ElapsedMilliseconds}", key, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to remove key {CacheKey}", key);
                throw;
            }
        }
    }
}
