using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlayerBack.Infrastructure.Models
{
    public class RepositoryCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public DateTime Created { get; internal set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; }
    }
}