using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleConcepts.DistributedDictionary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedDictionary(this IServiceCollection services)
        {
            return services.AddDistributedDictionary(opt => { });
        }

        public static IServiceCollection AddDistributedDictionary(this IServiceCollection services, Action<DistributedDictionaryOptions> configureOptions)
        {
            services.AddOptions();

            services.TryAddSingleton<IDistributedDictionaryFactory, DefaultDistributedDictionaryFactory>();
            services.TryAdd(ServiceDescriptor.Describe(typeof(IDistributedDictionary<,>), typeof(DistributedDictionary<,>), ServiceLifetime.Singleton));
            services.Configure(configureOptions);

            return services;
        }

        public static IServiceCollection AddDistributedDictionary<TKey, TValue>(this IServiceCollection services, Action<DistributedDictionaryOptions> configureOptions)
        {
            services.AddDistributedDictionary();
            services.Configure(DefaultDistributedDictionaryFactory.GetOptionsName<TKey, TValue>(), configureOptions);
            services.TryAddSingleton(provider => provider.GetRequiredService<IDistributedDictionaryFactory>().CreateDistributedDictionary<TKey, TValue>());

            return services;
        }

        public static IServiceCollection AddDistributedDictionary(this IServiceCollection services, string name, Action<DistributedDictionaryOptions> configureOptions)
        {
            services.AddDistributedDictionary();
            services.Configure(name, configureOptions);

            return services;
        }
    }
}
