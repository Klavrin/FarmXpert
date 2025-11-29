using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Domain.Entities
{
    public class ApplicationDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        public string FileExtension { get; set; } = string.Empty;
        [Required]
        public string OwnerId { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
    }
}
