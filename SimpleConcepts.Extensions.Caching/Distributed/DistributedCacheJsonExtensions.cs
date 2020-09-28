using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheJsonExtensions
    {
        public static T? GetJsonObject<T>(this IDistributedCache cache, string key) where T : class
        {
            return cache.GetJsonObject<T>(key, null);
        }

        public static T? GetJsonObject<T>(this IDistributedCache cache, string key, JsonSerializerOptions? serializerOptions) where T : class
        {
            var bytes = cache.Get(key);
            var result = bytes != null ? JsonSerializer.Deserialize<T>(bytes, serializerOptions) : null;

            return result;
        }

        public static Task<T?> GetJsonObjectAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default) where T : class
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

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value) where T : class
        {
            cache.SetJsonObject(key, value, new DistributedCacheEntryOptions(), null);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value, DistributedCacheEntryOptions entryOptions) where T : class
        {
            cache.SetJsonObject(key, value, entryOptions, null);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value, JsonSerializerOptions? serializerOptions) where T : class
        {
            cache.SetJsonObject(key, value, new DistributedCacheEntryOptions(), serializerOptions);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T? value, DistributedCacheEntryOptions entryOptions,
            JsonSerializerOptions? serializerOptions) where T : class
        {
            var bytes = value != null ? JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions) : null;

            cache.Set(key, bytes, entryOptions);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, JsonSerializerOptions serializerOptions,
            CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value, new DistributedCacheEntryOptions(), serializerOptions, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value, DistributedCacheEntryOptions entryOptions,
            CancellationToken token = default) where T : class
        {
            return cache.SetJsonObjectAsync(key, value, entryOptions, null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T? value, DistributedCacheEntryOptions entryOptions,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default) where T : class
        {
            var bytes = value != null ? JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions) : null;

            return cache.SetAsync(key, bytes, entryOptions, token);
        }


        public static T GetOrFetchJsonObject<T>(this IDistributedCache cache, string key, Func<T> fetchCallback) where T : class
        {
            return cache.GetOrFetchJsonObject(key, fetchCallback, new DistributedCacheEntryOptions());
        }

        public static T GetOrFetchJsonObject<T>(this IDistributedCache cache, string key, Func<T> fetchCallback,
            DistributedCacheEntryOptions options) where T : class
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
            Func<Task<T>> fetchCallback, CancellationToken token = default) where T : class
        {
            return cache.GetOrFetchJsonObjectAsync(key, fetchCallback, new DistributedCacheEntryOptions(), token);
        }

        public static async Task<T> GetOrFetchJsonObjectAsync<T>(this IDistributedCache cache, string key,
            Func<Task<T>> fetchCallback, DistributedCacheEntryOptions options, CancellationToken token = default) where T : class
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