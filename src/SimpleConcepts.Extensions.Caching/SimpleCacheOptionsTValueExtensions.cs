using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleCacheOptionsTValueExtensions
    {
        public static SimpleCacheOptions<TValue> WithKeyPrefix<TValue>(this SimpleCacheOptions<TValue> options, string keyPrefix)
        {
            options.KeySpace = keyPrefix + ":";

            return options;
        }

        public static SimpleCacheOptions<TValue> WithKeySerializer<TValue>(this SimpleCacheOptions<TValue> options, IKeySerializer keySerializer)
        {
            options.KeySerializer = keySerializer;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithValueSerializer<TValue>(this SimpleCacheOptions<TValue> options, IValueSerializer valueSerializer)
        {
            options.ValueSerializer = valueSerializer;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithDefaultEntryOptions<TValue>(this SimpleCacheOptions<TValue> options, DistributedCacheEntryOptions entryOptions)
        {
            options.DefaultEntryOptions = entryOptions;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithAbsoluteExpiration<TValue>(this SimpleCacheOptions<TValue> options, DateTimeOffset absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpiration = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithAbsoluteExpirationRelativeToNow<TValue>(this SimpleCacheOptions<TValue> options, TimeSpan absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpirationRelativeToNow = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithSlidingExpiration<TValue>(this SimpleCacheOptions<TValue> options, TimeSpan slidingExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.SlidingExpiration = slidingExpiration;

            return options;
        }

        public static SimpleCacheOptions<TValue> WithValueFactory<TValue>(
            this SimpleCacheOptions<TValue> options, Func<IServiceProvider, CancellationToken, Task<TValue>> valueFactory)
        {
            options.ValueFactory = valueFactory;

            return options;
        }
        public static SimpleCacheOptions<TValue> WithValueFactory<TValue>(
            this SimpleCacheOptions<TValue> options, Func<Task<TValue>> valueFactory)
        {
            return options
                .WithValueFactory((provider, token) => valueFactory());
        }
    }
}