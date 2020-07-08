using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatMash.Models
{
    public class Cat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("url")]
        public string Image { get; set; }

        [BsonElement("id")]
        public string legacyId { get; set; }

        [BsonElement("elo")]
        public int Elo { get; set; }

        [BsonElement("occurences")]
        public int Occurences { get; set; }
    }
}
