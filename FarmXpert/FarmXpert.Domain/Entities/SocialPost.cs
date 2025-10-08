using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FarmXpert.Domain.Entities;

public class SocialPost
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Url { get; set; }

    [Required]
    public string BusinessId { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = string.Empty;
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
}
