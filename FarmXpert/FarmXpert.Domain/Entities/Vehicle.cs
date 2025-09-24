using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Domain.Entities
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid BusinessId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid VehicleGroupId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public short FabricationDate { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}
