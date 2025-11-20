using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Domain.Entities
{
    public class ApplicationFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid DraftId { get; set; }
        public string SrcUrl { get; set; }
        public string OutPath { get; set; }
        public string Extension { get; set; }
    }
}
