using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlayerBack.Domain.Models
{
    public class RepositoryCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
    }
}