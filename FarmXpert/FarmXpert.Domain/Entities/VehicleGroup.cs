using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Domain.Entities
{
    public class VehicleGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid BusinessId { get; set; }
        public List<string> Vehicles;
        public string VehicleType { get; set; }
        public int Amount { get; set; }
    }
}
