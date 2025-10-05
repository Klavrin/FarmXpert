using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmXpert.Domain.Entities
{
    public class PersonalDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        [Required]
        public string Url { get; set; }
        [Required]
        public string FileExtension { get; set; }
        public string OwnerId { get; set; } = string.Empty;
    }
}
