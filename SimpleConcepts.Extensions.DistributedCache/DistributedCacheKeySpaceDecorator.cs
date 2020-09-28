using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.Caching
{
    public class DistributedCacheKeySpaceDecorator : IDistributedCache
    {
        private readonly string _prefix;
        private readonly IDistributedCache _cache;

        public DistributedCacheKeySpaceDecorator(IDistributedCache cache, IOptions<DistributedCacheKeySpaceOptions> options)
        {
            _prefix = options.Value.KeySpace ?? string.Empty;
            _cache = cache;
        }

        public byte[] Get(string key)
        {
            return _cache.Get(GetKey(key));
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            return _cache.GetAsync(GetKey(key), token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _cache.SetAsync(GetKey(key), value, options);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            return _cache.SetAsync(GetKey(key), value, options, token);
        }

        public void Refresh(string key)
        {
            _cache.Refresh(GetKey(key));
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return _cache.RefreshAsync(GetKey(key), token);
        }

        public void Remove(string key)
        {
            _cache.Remove(GetKey(key));
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return _cache.RemoveAsync(GetKey(key), token);
        }

        private string GetKey(string key)
        {
            return _prefix + key;
        }
    }
}
