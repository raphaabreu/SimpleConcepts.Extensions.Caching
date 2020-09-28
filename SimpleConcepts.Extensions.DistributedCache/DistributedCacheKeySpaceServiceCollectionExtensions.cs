using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedCacheKeySpaceServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDistributedCacheKeySpace(this IServiceCollection services, Action<DistributedCacheKeySpaceOptions> configureAction)
        {
            var descriptor = services.LastOrDefault(s => s.ServiceType == typeof(IDistributedCache));

            if (descriptor == null)
            {
                throw new InvalidOperationException($"You must have a {nameof(IDistributedCache)} configured before calling {nameof(ConfigureDistributedCacheKeySpace)}.");
            }

            services.Configure(configureAction);
            services.Decorate<IDistributedCache, DistributedCacheKeySpaceDecorator>();

            return services;
        }

        public static IServiceCollection ConfigureDistributedCacheKeySpace(this IServiceCollection services, string keySpace)
        {
            services.ConfigureDistributedCacheKeySpace(opt => opt.KeySpace = keySpace + ":");

            return services;
        }
    }
}