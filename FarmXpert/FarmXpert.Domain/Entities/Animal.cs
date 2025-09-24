using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmXpert.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FarmXpert.Domain.Entities
{
    public class Animal
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CattleId { get; set; }
        public string Species { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
    }

}
