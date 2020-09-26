using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleCache(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAddSingleton<ISimpleCacheFactory, SimpleCacheFactory>();

            return services;
        }

        public static IServiceCollection AddSimpleCache<TValue>(this IServiceCollection services)
        {
            services.AddSimpleCache<TValue>(Options.Options.DefaultName, opt => { });

            return services;
        }

        public static IServiceCollection AddSimpleCache<TValue>(this IServiceCollection services, Action<SimpleCacheOptions> configureOptions)
        {
            services.AddSimpleCache<TValue>(Options.Options.DefaultName, configureOptions);

            return services;
        }

        public static IServiceCollection AddSimpleCache<TValue>(this IServiceCollection services, string name, Action<SimpleCacheOptions> configureOptions)
        {
            services.AddSimpleCache();

            services.Configure(SimpleCacheFactory.GetOptionsName<TValue>(name), configureOptions);

            services.TryAddSingleton(provider => provider.GetRequiredService<ISimpleCacheFactory>().Create<TValue>(name));

            return services;
        }

        public static IServiceCollection AddSimpleCache<TKey, TValue>(this IServiceCollection services)
        {
            services.AddSimpleCache<TKey, TValue>(Options.Options.DefaultName, opt => { });

            return services;
        }

        public static IServiceCollection AddSimpleCache<TKey, TValue>(this IServiceCollection services, Action<SimpleCacheOptions> configureOptions)
        {
            services.AddSimpleCache<TKey, TValue>(Options.Options.DefaultName, configureOptions);

            return services;
        }

        public static IServiceCollection AddSimpleCache<TKey, TValue>(this IServiceCollection services, string name, Action<SimpleCacheOptions> configureOptions)
        {
            services.AddSimpleCache();

            services.Configure(SimpleCacheFactory.GetOptionsName<TKey, TValue>(name), configureOptions);

            services.TryAddSingleton(provider => provider.GetRequiredService<ISimpleCacheFactory>().Create<TKey, TValue>(name));

            return services;
        }
    }
}