using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.DistributedDictionary
{
    public interface IDistributedDictionary<in TKey, TValue>
    {
        Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task RefreshAsync(TKey key, CancellationToken cancellationToken = default);
        Task RemoveAsync(TKey key, CancellationToken cancellationToken = default);
        Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
        Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions entryOptions, CancellationToken cancellationToken = default);
    }
}
