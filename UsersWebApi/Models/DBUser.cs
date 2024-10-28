using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UsersWebApi.Models
{
    public class DBUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Login { get; set; }

        public string HashedPassword { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiredAt { get; set; }

        public int RegisteredObjects { get; set; }
    }
}