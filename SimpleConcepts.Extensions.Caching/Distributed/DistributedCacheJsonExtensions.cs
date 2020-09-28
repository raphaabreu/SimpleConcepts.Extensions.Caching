using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheJsonExtensions
    {
        public static T GetJsonObject<T>(this IDistributedCache cache, string key)
        {
            return cache.GetJsonObject<T>(key, null);
        }

        public static T GetJsonObject<T>(this IDistributedCache cache, string key, JsonSerializerOptions? serializerOptions)
        {
            var bytes = cache.Get(key);
            var result = JsonSerializer.Deserialize<T>(bytes, serializerOptions);

            return result;
        }

        public static Task<T> GetJsonObjectAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            return cache.GetJsonObjectAsync<T>(key, null, token);
        }

        public static async Task<T> GetJsonObjectAsync<T>(this IDistributedCache cache, string key,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default)
        {
            var bytes = await cache.GetAsync(key, token);
            var result = JsonSerializer.Deserialize<T>(bytes, serializerOptions);

            return result;
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value)
        {
            cache.SetJsonObject(key, value, new DistributedCacheEntryOptions(), null);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions entryOptions)
        {
            cache.SetJsonObject(key, value, entryOptions, null);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value, JsonSerializerOptions? serializerOptions)
        {
            cache.SetJsonObject(key, value, new DistributedCacheEntryOptions(), serializerOptions);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions entryOptions,
            JsonSerializerOptions? serializerOptions)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions);

            cache.Set(key, bytes, entryOptions);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken token = default)
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, JsonSerializerOptions serializerOptions,
            CancellationToken token = default)
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), serializerOptions, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions entryOptions,
            CancellationToken token = default)
        {
            return cache.SetJsonObjectAsync(key, value, entryOptions, null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions entryOptions,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions);

            return cache.SetAsync(key, bytes, entryOptions, token);
        }


        public static T GetOrFetchJsonObject<T>(this IDistributedCache cache, string key, Func<T> fetchCallback)
        {
            return cache.GetOrFetchJsonObject(key, fetchCallback, new DistributedCacheEntryOptions());
        }

        public static T GetOrFetchJsonObject<T>(this IDistributedCache cache, string key, Func<T> fetchCallback,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.GetJsonObject<T>(key);

            if (cached != null)
            {
                return cached;
            }

            var value = fetchCallback();

            cache.SetJsonObject(key, value, options);

            return value;
        }

        public static Task<T> GetOrFetchJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> fetchCallback, CancellationToken token = default)
        {
            return cache.GetOrFetchJsonObjectAsync(key, fetchCallback, new DistributedCacheEntryOptions(), token);
        }

        public static async Task<T> GetOrFetchJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> fetchCallback, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var cached = await cache.GetJsonObjectAsync<T>(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetJsonObjectAsync(key, value, options, token);

            return value;
        }
    }
}