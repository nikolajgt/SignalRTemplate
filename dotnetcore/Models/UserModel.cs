using MongoDB.Bson.Serialization.Attributes;
using System;

namespace dotnetcore.Models
{
    public class UserModel
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public bool LoggedIn { get; set; }
    }
}
