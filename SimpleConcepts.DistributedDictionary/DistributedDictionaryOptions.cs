using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.DistributedDictionary
{
    public class DistributedDictionaryOptions
    {
        public string KeyNamespace { get; set; }
        public string KeyPrefix { get; set; }
        public IKeySerializer KeySerializer { get; set; }
        public IValueSerializer ValueSerializer { get; set; }
        public DistributedCacheEntryOptions DefaultEntryOptions { get; set; }
    }
}