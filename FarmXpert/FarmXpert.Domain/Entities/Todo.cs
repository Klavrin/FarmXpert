using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Domain.Entities;

public class Todo
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
