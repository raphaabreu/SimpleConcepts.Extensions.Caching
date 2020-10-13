using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheJsonExtensions
    {
        public static T? GetJsonObject<T>(this IDistributedCache cache, string key,
            JsonSerializerOptions? serializerOptions = null) where T : class
        {
            var bytes = cache.Get(key);
            var result = bytes != null ? JsonSerializer.Deserialize<T>(bytes, serializerOptions) : null;

            return result;
        }

        public static Task<T?> GetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            CancellationToken token = default) where T : class
        {
            return cache.GetJsonObjectAsync<T>(key, null, token);
        }

        public static async Task<T?> GetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default) where T : class
        {
            var bytes = await cache.GetAsync(key, token);
            var result = bytes != null ? JsonSerializer.Deserialize<T>(bytes, serializerOptions) : null;

            return result;
        }




        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value,
            JsonSerializerOptions? serializerOptions = null) where T : class
        {
            cache.SetJsonObject(key, value, new DistributedCacheEntryOptions(), serializerOptions);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value,
            TimeSpan absoluteExpirationRelativeToNow, JsonSerializerOptions? serializerOptions = null) where T : class
        {
            cache.SetJsonObject(key, value,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                serializerOptions);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value,
            DistributedCacheEntryOptions entryOptions, JsonSerializerOptions? serializerOptions = null) where T : class
        {
            var bytes = value != null ? JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions) : null;

            cache.Set(key, bytes, entryOptions);
        }


        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value,
            CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), serializerOptions, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value,
            TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value,
            TimeSpan absoluteExpirationRelativeToNow, JsonSerializerOptions? serializerOptions,
            CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                serializerOptions, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value,
            DistributedCacheEntryOptions entryOptions, JsonSerializerOptions? serializerOptions = null,
            CancellationToken token = default) where T : class
        {
            var bytes = value != null ? JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions) : null;

            return cache.SetAsync(key, bytes, entryOptions, token);
        }



        public static T GetOrSetJsonObject<T>(this IDistributedCache cache, string key, Func<T> valueFactory,
            JsonSerializerOptions? serializerOptions = null) where T : class
        {
            return cache.GetOrSetJsonObject(key, valueFactory, new DistributedCacheEntryOptions(), serializerOptions);
        }


        public static T GetOrSetJsonObject<T>(this IDistributedCache cache, string key, Func<T> valueFactory,
            TimeSpan absoluteExpirationRelativeToNow, JsonSerializerOptions? serializerOptions = null) where T : class
        {
            return cache.GetOrSetJsonObject<T>(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                serializerOptions);
        }

        public static T GetOrSetJsonObject<T>(this IDistributedCache cache, string key, Func<T> valueFactory,
            DistributedCacheEntryOptions entryOptions, JsonSerializerOptions? serializerOptions = null) where T : class
        {
            var cached = cache.GetJsonObject<T>(key, serializerOptions);

            if (cached != null)
            {
                return cached;
            }

            var value = valueFactory();

            cache.SetJsonObject(key, value, entryOptions, serializerOptions);

            return value;
        }


        public static Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, CancellationToken token = default) where T : class
        {
            return cache.GetOrSetJsonObjectAsync(key, valueFactory, new DistributedCacheEntryOptions(), null, token);
        }
        public static Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, JsonSerializerOptions? serializerOptions,
            CancellationToken token = default) where T : class
        {
            return cache.GetOrSetJsonObjectAsync(key, valueFactory, new DistributedCacheEntryOptions(), serializerOptions, token);
        }

        public static Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, TimeSpan absoluteExpirationRelativeToNow, JsonSerializerOptions? serializerOptions,
            CancellationToken token = default) where T : class
        {
            return cache.GetOrSetJsonObjectAsync(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                serializerOptions, token);
        }

        public static Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default) where T : class
        {
            return cache.GetOrSetJsonObjectAsync(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow },
                null, token);
        }

        public static Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, DistributedCacheEntryOptions entryOptions,
            CancellationToken token = default) where T : class
        {
            return cache.GetOrSetJsonObjectAsync<T>(key, valueFactory, entryOptions, null, token);
        }

        public static async Task<T> GetOrSetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> valueFactory, DistributedCacheEntryOptions entryOptions,
            JsonSerializerOptions? serializerOptions = null, CancellationToken token = default) where T : class
        {
            var cached = await cache.GetJsonObjectAsync<T>(key, serializerOptions, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            await cache.SetJsonObjectAsync(key, value, entryOptions, serializerOptions, token);

            return value;
        }
    }
}