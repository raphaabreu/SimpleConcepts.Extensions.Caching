using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SimpleConcepts.DistributedDictionary.Tests
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddDistributedDictionary_WithOptions_CanCreateServicesCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();

            // Act
            services.AddDistributedDictionary(opts => opts.KeyNamespace = "TestingNamespace");

            // Assert
            var provider = services.BuildServiceProvider();
            var dic1 = provider.GetService<IDistributedDictionary<Guid, TestPerson>>();
            var dic2 = provider.GetService<IDistributedDictionary<string, int>>();
            var dic3 = provider.GetService<IDistributedDictionary<int, Guid>>();
            Assert.NotNull(dic1);
            Assert.NotNull(dic2);
            Assert.NotNull(dic3);
        }

        [Fact]
        public void AddDistributedDictionary_TypedWithCustomOptions_CanCreateServicesCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();

            // Act
            services.AddDistributedDictionary(opts => opts.KeyNamespace = "TestingNamespace");
            services.AddDistributedDictionary<int, Guid>(opts => opts.KeyPrefix = "TestPrefix");

            // Assert
            var provider = services.BuildServiceProvider();
            var dic = provider.GetService<IDistributedDictionary<int, Guid>>();
            Assert.NotNull(dic);
        }

        [Fact]
        public void AddDistributedDictionary_NamedWithCustomOptions_CanCreateServicesCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();

            // Act
            services.AddDistributedDictionary(opts => opts.KeyNamespace = "TestingNamespace");
            services.AddDistributedDictionary("customDic", opts => opts.KeyPrefix = "customDic");

            // Assert
            var provider = services.BuildServiceProvider();
            var fac = provider.GetService<IDistributedDictionaryFactory>();
            var dic = fac.Create<int, int>("customDic");
            Assert.NotNull(dic);
        }
    }
}