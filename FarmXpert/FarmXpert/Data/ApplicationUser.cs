using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Data;

public class ApplicationUser: MongoUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string BusinessId { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}