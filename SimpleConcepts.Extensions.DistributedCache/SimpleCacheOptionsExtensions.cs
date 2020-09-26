using System;
using Microsoft.Extensions.Caching.Distributed;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleCacheOptionsExtensions
    {
        public static SimpleCacheOptions WithKeyPrefix(this SimpleCacheOptions options, string keyPrefix)
        {
            options.KeySpace = keyPrefix;

            return options;
        }

        public static SimpleCacheOptions WithKeySerializer(this SimpleCacheOptions options, IKeySerializer keySerializer)
        {
            options.KeySerializer = keySerializer;

            return options;
        }

        public static SimpleCacheOptions WithValueSerializer(this SimpleCacheOptions options, IValueSerializer valueSerializer)
        {
            options.ValueSerializer = valueSerializer;

            return options;
        }

        public static SimpleCacheOptions WithDefaultEntryOptions(this SimpleCacheOptions options, DistributedCacheEntryOptions entryOptions)
        {
            options.DefaultEntryOptions = entryOptions;

            return options;
        }

        public static SimpleCacheOptions WithAbsoluteExpiration(this SimpleCacheOptions options, DateTimeOffset absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpiration = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions WithAbsoluteExpirationRelativeToNow(this SimpleCacheOptions options, TimeSpan absoluteExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.AbsoluteExpirationRelativeToNow = absoluteExpiration;

            return options;
        }

        public static SimpleCacheOptions WithSlidingExpiration(this SimpleCacheOptions options, TimeSpan slidingExpiration)
        {
            if (options.DefaultEntryOptions == null)
            {
                options.DefaultEntryOptions = new DistributedCacheEntryOptions();
            }

            options.DefaultEntryOptions.SlidingExpiration = slidingExpiration;

            return options;
        }
    }
}