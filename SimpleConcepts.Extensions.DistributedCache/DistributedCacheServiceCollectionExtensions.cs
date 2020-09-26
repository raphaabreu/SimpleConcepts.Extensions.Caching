using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public static class DistributedCacheServiceCollectionExtensions
    {
        private static void AddDistributedCacheInternal(this IServiceCollection services)
        {
            services.AddOptions();

            services.TryAddSingleton<IDistributedCacheFactory, DistributedCacheFactory>();
            services.TryAdd(ServiceDescriptor.Describe(typeof(IDistributedCache<,>), typeof(DistributedCache<,>), ServiceLifetime.Scoped));
        }

        public static IServiceCollection AddDistributedCache<TKey, TValue>(this IServiceCollection services)
        {
            services.AddDistributedCacheInternal();
            services.TryAddSingleton(provider => provider.GetRequiredService<IDistributedCacheFactory>().Create<TKey, TValue>());

            return services;
        }

        public static IServiceCollection AddDistributedCache<TKey, TValue>(this IServiceCollection services, Action<DistributedCacheOptions> configureOptions)
        {
            services.AddDistributedCache<TKey, TValue>();
            services.Configure(DistributedCacheFactory.GetOptionsName<TKey, TValue>(), configureOptions);

            return services;
        }

        public static IServiceCollection ConfigureDistributedCache(this IServiceCollection services, string name, Action<DistributedCacheOptions> configureOptions)
        {
            services.AddDistributedCacheInternal();
            services.Configure(name, configureOptions);

            return services;
        }
    }
}