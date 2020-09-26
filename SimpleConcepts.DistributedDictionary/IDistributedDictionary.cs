using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.DistributedDictionary
{
    public interface IDistributedDictionary<in TKey, TValue>
    {
        Task<bool> ContainsKeyAsync(TKey key, CancellationToken cancellationToken = default);
        Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task RemoveAsync(TKey key, CancellationToken cancellationToken = default);
        Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    }
}
