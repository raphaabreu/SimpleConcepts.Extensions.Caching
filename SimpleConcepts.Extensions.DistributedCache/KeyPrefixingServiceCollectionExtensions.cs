using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public static class KeyPrefixingServiceCollectionExtensions
    {
        public static IServiceCollection AddKeyPrefixingToDistributedCache(this IServiceCollection services, string prefix)
        {
            var descriptor = services.LastOrDefault(s => s.ServiceType == typeof(IDistributedCache));

            if (descriptor == null)
            {
                throw new InvalidOperationException($"You must have a {nameof(IDistributedCache)} configured before calling {nameof(AddKeyPrefixingToDistributedCache)}.");
            }

            services.Configure<KeyPrefixingOptions>(options => options.Prefix = prefix);
            services.Decorate<IDistributedCache, KeyPrefixingDecorator>();

            return services;
        }
    }
}