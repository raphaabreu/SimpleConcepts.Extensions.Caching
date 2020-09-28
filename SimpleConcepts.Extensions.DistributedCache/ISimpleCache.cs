using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public interface ISimpleCache<TValue> where TValue : class
    {
        Task<TValue?> GetAsync(CancellationToken token = default);
        Task RefreshAsync(CancellationToken token = default);
        Task RemoveAsync(CancellationToken token = default);
        Task SetAsync(TValue value, CancellationToken token = default);
        Task SetAsync(TValue value, DistributedCacheEntryOptions options, CancellationToken token = default);
    }

    public interface ISimpleCache<in TKey, TValue> where TValue : class
    {
        Task<TValue?> GetAsync(TKey key, CancellationToken token = default);
        Task RefreshAsync(TKey key, CancellationToken token = default);
        Task RemoveAsync(TKey key, CancellationToken token = default);
        Task SetAsync(TKey key, TValue value, CancellationToken token = default);
        Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions options, CancellationToken token = default);
    }
}