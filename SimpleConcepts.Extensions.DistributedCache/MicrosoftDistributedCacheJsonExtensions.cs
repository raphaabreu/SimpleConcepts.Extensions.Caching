using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class MicrosoftDistributedCacheJsonExtensions
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

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            cache.SetJsonObject(key, value, options, null);
        }

        public static void SetJsonObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options,
            JsonSerializerOptions? serializerOptions)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions);

            cache.Set(key, bytes, options);
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

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            return cache.SetJsonObjectAsync(key, value, options, null, token);
        }

        public static Task SetJsonObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options,
            JsonSerializerOptions? serializerOptions, CancellationToken token = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions);

            return cache.SetAsync(key, bytes, options, token);
        }
    }
}