using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public interface IDistributedCache<in TKey, TValue>
    {
        TValue Get(TKey key);
        Task<TValue> GetAsync(TKey key, CancellationToken token = default);
        void Refresh(TKey key);
        Task RefreshAsync(TKey key, CancellationToken token = default);
        void Remove(TKey key);
        Task RemoveAsync(TKey key, CancellationToken token = default);
        void Set(TKey key, TValue value, DistributedCacheEntryOptions options);
        Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions options, CancellationToken token = default);
    }
}