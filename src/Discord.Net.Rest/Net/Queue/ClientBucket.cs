using System.Collections.Immutable;

namespace Discord.Net.Queue
{
    public enum ClientBucketType
    {
        Unbucketed = 0,
        SendEdit = 1
    }
    internal struct ClientBucket
    {
        private static readonly ImmutableDictionary<ClientBucketType, ClientBucket> DefsByType;
        private static readonly ImmutableDictionary<string, ClientBucket> DefsById;

        static ClientBucket()
        {
            var buckets = new[]
            {
                new ClientBucket(ClientBucketType.Unbucketed, "<unbucketed>", 10, 10),
                new ClientBucket(ClientBucketType.SendEdit, "<send_edit>", 10, 10)
            };

            var builder = ImmutableDictionary.CreateBuilder<ClientBucketType, ClientBucket>();
            foreach (var bucket in buckets)
                builder.Add(bucket.Type, bucket);
            DefsByType = builder.ToImmutable();

            var builder2 = ImmutableDictionary.CreateBuilder<string, ClientBucket>();
            foreach (var bucket in buckets)
                builder2.Add(bucket.Id, bucket);
            DefsById = builder2.ToImmutable();
        }

        public static ClientBucket Get(ClientBucketType type) => DefsByType[type];
        public static ClientBucket Get(string id) => DefsById[id];
        
        public ClientBucketType Type { get; }
        public string Id { get; }
        public int WindowCount { get; }
        public int WindowSeconds { get; }

        public ClientBucket(ClientBucketType type, string id, int count, int seconds)
        {
            Type = type;
            Id = id;
            WindowCount = count;
            WindowSeconds = seconds;
        }
    }
}
