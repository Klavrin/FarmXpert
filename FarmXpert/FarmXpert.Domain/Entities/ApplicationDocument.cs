using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Domain.Entities
{
    public class ApplicationDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid ApplicationId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid TemplateId { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public string Status { get; set; }
        public object AiFilledPayload { get; set; } = new();
        public string RejectionReason { get; set; }
        public string StoragePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
