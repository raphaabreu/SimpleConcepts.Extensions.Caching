using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCacheFactory : ISimpleCacheFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IOptionsMonitor<SimpleCacheOptions> _optionsMonitor;

        public SimpleCacheFactory(IServiceProvider provider, IOptionsMonitor<SimpleCacheOptions> optionsMonitor)
        {
            _provider = provider;
            _optionsMonitor = optionsMonitor;
        }

        public ISimpleCache<TValue> Create<TValue>()
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TValue>(cache, opts);
        }

        public ISimpleCache<TValue> Create<TValue>(string name)
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TValue>(name));
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TValue>(cache, opts);
        }

        public ISimpleCache<TKey, TValue> Create<TKey, TValue>()
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TKey, TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TKey, TValue>(cache, opts);
        }

        public ISimpleCache<TKey, TValue> Create<TKey, TValue>(string name)
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TKey, TValue>(name));
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TKey, TValue>(cache, opts);
        }

        public static string GetOptionsName<TValue>()
        {
            return GetOptionsName<TValue>(Options.DefaultName);
        }

        public static string GetOptionsName<TKey, TValue>()
        {
            return GetOptionsName<TKey, TValue>(Options.DefaultName);
        }

        public static string GetOptionsName<TValue>(string name)
        {
            return $"{typeof(TValue).FullName}:{name}";
        }

        public static string GetOptionsName<TKey, TValue>(string name)
        {
            return $"{typeof(TKey).FullName}:{typeof(TValue).FullName}:{name}";
        }
    }
}