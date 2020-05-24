using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.DistributedDictionary
{
    internal class DefaultDistributedDictionaryFactory : IDistributedDictionaryFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IOptionsMonitor<DistributedDictionaryOptions> _optionsMonitor;

        public DefaultDistributedDictionaryFactory(IServiceProvider provider, IOptionsMonitor<DistributedDictionaryOptions> optionsMonitor)
        {
            _provider = provider;
            _optionsMonitor = optionsMonitor;
        }

        public IDistributedDictionary<TKey, TValue> CreateDefaultDistributedDictionary<TKey, TValue>()
        {
            var opts = _optionsMonitor.CurrentValue;
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedDictionary<TKey, TValue>(cache, new OptionsWrapper<DistributedDictionaryOptions>(opts));
        }

        public IDistributedDictionary<TKey, TValue> CreateDistributedDictionary<TKey, TValue>()
        {
            var opts = _optionsMonitor.Get(GetOptionsName<TKey, TValue>());
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedDictionary<TKey, TValue>(cache, new OptionsWrapper<DistributedDictionaryOptions>(opts));
        }

        public IDistributedDictionary<TKey, TValue> CreateDistributedDictionary<TKey, TValue>(string name)
        {
            var opts = _optionsMonitor.Get(name);
            var cache = _provider.GetRequiredService<IDistributedCache>();

            return new DistributedDictionary<TKey, TValue>(cache, new OptionsWrapper<DistributedDictionaryOptions>(opts));
        }

        public static string GetOptionsName<TKey, TValue>()
        {
            return $"{typeof(TKey).FullName}:{typeof(TValue).FullName}";
        }
    }
}