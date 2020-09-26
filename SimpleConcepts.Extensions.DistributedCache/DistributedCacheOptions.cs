using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public class DistributedCacheOptions
    {
        public string KeyPrefix { get; set; }
        public IKeySerializer KeySerializer { get; set; }
        public IValueSerializer ValueSerializer { get; set; }
        public DistributedCacheEntryOptions DefaultEntryOptions { get; set; }
    }
}