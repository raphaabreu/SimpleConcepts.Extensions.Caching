using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCacheFactory : ISimpleCacheFactory
    {
        private readonly IServiceProvider _provider;

        public SimpleCacheFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ISimpleCache<TValue> Create<TValue>() where TValue : class
        {
            var optionsMonitor = _provider.GetRequiredService<IOptionsMonitor<SimpleCacheOptions<TValue>>>();
            var opts = optionsMonitor.Get(GetOptionsName<TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TValue>(cache, _provider, opts);
        }

        public ISimpleCache<TValue> Create<TValue>(string name) where TValue : class
        {
            var optionsMonitor = _provider.GetRequiredService<IOptionsMonitor<SimpleCacheOptions<TValue>>>();
            var opts = optionsMonitor.Get(GetOptionsName<TValue>(name));
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TValue>(cache, _provider, opts);
        }

        public ISimpleCache<TKey, TValue> Create<TKey, TValue>() where TValue : class
        {
            var optionsMonitor = _provider.GetRequiredService<IOptionsMonitor<SimpleCacheOptions<TKey, TValue>>>();
            var opts = optionsMonitor.Get(GetOptionsName<TKey, TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TKey, TValue>(cache, _provider, opts);
        }

        public ISimpleCache<TKey, TValue> Create<TKey, TValue>(string name) where TValue : class
        {
            var optionsMonitor = _provider.GetRequiredService<IOptionsMonitor<SimpleCacheOptions<TKey, TValue>>>();
            var opts = optionsMonitor.Get(GetOptionsName<TKey, TValue>(name));
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new SimpleCache<TKey, TValue>(cache, _provider, opts);
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