using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    internal class DistributedCacheFactory : IDistributedCacheFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IOptionsMonitor<DistributedCacheOptions> _optionsMonitor;

        public DistributedCacheFactory(IServiceProvider provider, IOptionsMonitor<DistributedCacheOptions> optionsMonitor)
        {
            _provider = provider;
            _optionsMonitor = optionsMonitor;
        }

        public IDistributedCache<TKey, TValue> CreateDefault<TKey, TValue>()
        {
            var opts = _optionsMonitor.CurrentValue;
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedCache<TKey, TValue>(cache, new OptionsWrapper<DistributedCacheOptions>(opts));
        }

        public IDistributedCache<TKey, TValue> Create<TKey, TValue>()
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TKey, TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedCache<TKey, TValue>(cache, new OptionsWrapper<DistributedCacheOptions>(opts));
        }

        public IDistributedCache<TKey, TValue> Create<TKey, TValue>(string name)
        {
            var opts = _optionsMonitor.Get(name);
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedCache<TKey, TValue>(cache, new OptionsWrapper<DistributedCacheOptions>(opts));
        }

        public static string GetOptionsName<TKey, TValue>()
        {
            return $"{typeof(TKey).FullName}:{typeof(TValue).FullName}";
        }
    }
}