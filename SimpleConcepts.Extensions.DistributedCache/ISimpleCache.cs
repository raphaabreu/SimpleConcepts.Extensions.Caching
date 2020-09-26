using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public interface ISimpleCache<TValue>
    {
        TValue Get();
        Task<TValue> GetAsync(CancellationToken token = default);
        void Refresh();
        Task RefreshAsync(CancellationToken token = default);
        void Remove();
        Task RemoveAsync(CancellationToken token = default);
        void Set(TValue value, DistributedCacheEntryOptions options = default);
        Task SetAsync(TValue value, DistributedCacheEntryOptions options = default, CancellationToken token = default);
    }

    public interface ISimpleCache<in TKey, TValue>
    {
        TValue Get(TKey key);
        Task<TValue> GetAsync(TKey key, CancellationToken token = default);
        void Refresh(TKey key);
        Task RefreshAsync(TKey key, CancellationToken token = default);
        void Remove(TKey key);
        Task RemoveAsync(TKey key, CancellationToken token = default);
        void Set(TKey key, TValue value, DistributedCacheEntryOptions options = default);
        Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions options = default, CancellationToken token = default);
    }
}