using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SportNewsWebApi.Models
{
    public class SportNews
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int? UserId { get; set; } = null;

        public DateTime? ConfirmationTime { get; set; } = null;
    }
}