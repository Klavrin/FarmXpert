using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmXpert.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FarmXpert.Domain.Entities
{
    public class Field
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid BusinessId { get; set; }
        public CropType CropType { get; set; }
        public string OtherCropType { get; set; } = string.Empty;
        public SoilType SoilType { get; set; }
        public string OtherSoilType { get; set; } = string.Empty;
        public FertilizerType Fertilizer { get; set; }
        public string OtherFertilizer { get; set; } = string.Empty;
        public HerbicideType Herbicide { get; set; }
        public string OtherHerbicide { get; set; } = string.Empty;
        public List<double[]> Coords { get; set; } = new();
        public string OwnerId { get; set; } = string.Empty;
    }
}
