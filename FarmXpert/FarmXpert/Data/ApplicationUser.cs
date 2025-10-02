using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mono.TextTemplating;

namespace FarmXpert.Data;

public class ApplicationUser: MongoUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string BusinessId { get; set; }
    public string FarmName { get; set; } = string.Empty;
    public List<LandParcel> LandParcels = new();
    public DateTime FarmDateOfRegistration { get; set; }
    public bool IsVerified { get; set; }
    public bool ProfileSetupCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class LandParcel
{
    public string Parcel { get; set; } = string.Empty;
    public double Size { get; set; }
}