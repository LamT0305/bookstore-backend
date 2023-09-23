using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStore.Models
{
	public class Publisher
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Address")]
        [BsonRequired]
        public string Address { get; set; } = string.Empty;

        [BsonElement("Phone")]
        [BsonRequired]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("Email")]
        [BsonRequired]
        public string Email { get; set; } = string.Empty;

        //[BsonRepresentation(BsonType.ObjectId)]
        //public List<string> Books { get; set; } = new List<string>();
    }
}

