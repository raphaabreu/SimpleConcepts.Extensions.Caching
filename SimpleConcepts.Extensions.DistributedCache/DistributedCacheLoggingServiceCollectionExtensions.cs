using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using SimpleConcepts.Extensions.Caching;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedCacheLoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedCacheLogging(this IServiceCollection services)
        {
            var descriptor = services.LastOrDefault(s => s.ServiceType == typeof(IDistributedCache));

            if (descriptor == null)
            {
                throw new InvalidOperationException($"You must have a {nameof(IDistributedCache)} configured before calling {nameof(AddDistributedCacheLogging)}.");
            }

            services.Decorate<IDistributedCache, DistributedCacheLoggingDecorator>();

            return services;
        }
    }
}