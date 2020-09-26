using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCacheOptions
    {
        public string KeySpace { get; set; }
        public IKeySerializer KeySerializer { get; set; }
        public IValueSerializer ValueSerializer { get; set; }
        public DistributedCacheEntryOptions DefaultEntryOptions { get; set; }
    }
}