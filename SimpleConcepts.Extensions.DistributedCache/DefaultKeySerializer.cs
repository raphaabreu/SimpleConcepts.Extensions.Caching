namespace SimpleConcepts.Extensions.Caching
{
    public class DefaultKeySerializer : IKeySerializer
    {
        public string Serialize(object key)
        {
            switch (key)
            {
                case null:
                    return "<NULL>";
                case string sKey:
                    return sKey;
                default:
                    return key.ToString();
            }
        }
    }
}