using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleCacheOptionsTKeyTValueExtensions
    {
        public static SimpleCacheOptions<TKey, TValue> WithKeyPrefix<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, string keyPrefix)
        {
            options.KeySpace = keyPrefix + ":";

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithKeySerializer<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, IKeySerializer keySerializer)
        {
            options.KeySerializer = keySerializer;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithValueSerializer<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, IValueSerializer valueSerializer)
        {
            options.ValueSerializer = valueSerializer;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithDefaultEntryOptions<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, DistributedCacheEntryOptions entryOptions)
        {
            options.DefaultEntryOptions = entryOptions;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithAbsoluteExpiration<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, DateTimeOffset absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpiration = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithAbsoluteExpirationRelativeToNow<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, TimeSpan absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpirationRelativeToNow = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithSlidingExpiration<TKey, TValue>(this SimpleCacheOptions<TKey, TValue> options, TimeSpan slidingExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.SlidingExpiration = slidingExpiration;

            return options;
        }

        public static SimpleCacheOptions<TKey, TValue> WithValueFactory<TKey, TValue>(
            this SimpleCacheOptions<TKey, TValue> options, Func<TKey, IServiceProvider, CancellationToken, Task<TValue>> valueFactory)
        {
            options.ValueFactory = valueFactory;

            return options;
        }
        public static SimpleCacheOptions<TKey, TValue> WithValueFactory<TKey, TValue>(
            this SimpleCacheOptions<TKey, TValue> options, Func<TKey, Task<TValue>> valueFactory)
        {
            return options
                .WithValueFactory((key, provider, token) => valueFactory(key));
        }
    }
}