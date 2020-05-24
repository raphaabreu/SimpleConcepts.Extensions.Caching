using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace SimpleConcepts.DistributedDictionary.Tests
{
    public class DistributedDictionaryTests
    {
        [Fact]
        public async Task SetAsync_WithDefaultOptions_PutsCorrectBytesOnCorrectKey()
        {
            // Arrange
            var cache = CreateDistributedCache();
            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");
            var value = new TestPerson { Name = "Raphael", Age = 31 };

            // Act
            await dic.SetAsync(key, value);

            // Assert
            var bytes = await cache.GetAsync("testhost:SimpleConcepts.DistributedDictionary.Tests.TestPerson:054392d5-1a51-4f8a-96c7-2a3eb0db2842");
            Assert.Equal(new byte[] { 123, 34, 78, 97, 109, 101, 34, 58, 34, 82, 97, 112, 104, 97, 101, 108, 34, 44, 34, 65, 103, 101, 34, 58, 51, 49, 125 }, bytes);
        }

        [Fact]
        public async Task GetAsync_WithDefaultOptions_ReturnsCorrectObject()
        {
            // Arrange
            var cache = CreateDistributedCache();
            await cache.SetAsync("testhost:SimpleConcepts.DistributedDictionary.Tests.TestPerson:054392d5-1a51-4f8a-96c7-2a3eb0db2842",
                new byte[] { 123, 34, 78, 97, 109, 101, 34, 58, 34, 82, 97, 112, 104, 97, 101, 108, 34, 44, 34, 65, 103, 101, 34, 58, 51, 49, 125 });

            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");

            // Act
            var result = await dic.GetAsync(key);

            // Assert
            Assert.Equal("Raphael", result.Name);
            Assert.Equal(31, result.Age);
        }

        [Fact]
        public async Task GetAsync_WithMissingKey_ThrowsKeyNotFound()
        {
            // Arrange
            var cache = CreateDistributedCache();
            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");

            // Act
            var act = dic.GetAsync(key);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => act);
        }

        [Fact]
        public async Task GetOrDefaultAsync_WithDefaultOptions_ReturnsCorrectObject()
        {
            // Arrange
            var cache = CreateDistributedCache();
            await cache.SetAsync("testhost:SimpleConcepts.DistributedDictionary.Tests.TestPerson:054392d5-1a51-4f8a-96c7-2a3eb0db2842",
                new byte[] { 123, 34, 78, 97, 109, 101, 34, 58, 34, 82, 97, 112, 104, 97, 101, 108, 34, 44, 34, 65, 103, 101, 34, 58, 51, 49, 125 });

            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");

            // Act
            var result = await dic.GetOrDefaultAsync(key);

            // Assert
            Assert.Equal("Raphael", result.Name);
            Assert.Equal(31, result.Age);
        }

        [Fact]
        public async Task GetOrDefaultAsync_WithMissingKey_ReturnsDefault()
        {
            // Arrange
            var cache = CreateDistributedCache();
            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");

            // Act
            var result = await dic.GetOrDefaultAsync(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_WithDefaultOptions_RemovesFromCache()
        {
            // Arrange
            var cache = CreateDistributedCache();
            await cache.SetAsync("testhost:SimpleConcepts.DistributedDictionary.Tests.TestPerson:054392d5-1a51-4f8a-96c7-2a3eb0db2842",
                new byte[] { 123, 34, 78, 97, 109, 101, 34, 58, 34, 82, 97, 112, 104, 97, 101, 108, 34, 44, 34, 65, 103, 101, 34, 58, 51, 49, 125 });

            var dic = new DistributedDictionary<Guid, TestPerson>(cache, new OptionsWrapper<DistributedDictionaryOptions>(new DistributedDictionaryOptions()));
            var key = Guid.Parse("054392d5-1a51-4f8a-96c7-2a3eb0db2842");

            // Act
            await dic.RemoveAsync(key);

            // Assert
            var bytes = await cache.GetAsync("testhost:SimpleConcepts.DistributedDictionary.Tests.TestPerson:054392d5-1a51-4f8a-96c7-2a3eb0db2842");
            Assert.Null(bytes);
        }

        private static IDistributedCache CreateDistributedCache()
        {
            var opts = new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());

            return new MemoryDistributedCache(opts);
        }
    }
}
