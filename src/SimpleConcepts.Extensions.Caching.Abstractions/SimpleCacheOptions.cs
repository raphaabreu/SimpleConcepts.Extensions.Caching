using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public abstract class SimpleCacheOptions
    {
        public string? KeySpace { get; set; }
        public IKeySerializer? KeySerializer { get; set; }
        public IValueSerializer? ValueSerializer { get; set; }
        public DistributedCacheEntryOptions? DefaultEntryOptions { get; set; }
        public bool FallbackToFactoryOnException { get; set; } = true;
    }

    public class SimpleCacheOptions<TKey, TValue> : SimpleCacheOptions where TValue : class
    {
        public Func<TKey, IServiceProvider, CancellationToken, Task<TValue?>>? ValueFactory { get; set; }
    }

    public class SimpleCacheOptions<TValue> : SimpleCacheOptions where TValue : class
    {
        public Func<IServiceProvider, CancellationToken, Task<TValue?>>? ValueFactory { get; set; }
    }
}