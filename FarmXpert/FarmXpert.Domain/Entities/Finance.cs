using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Domain.Entities
{
    public class Finance
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid BusinessId { get; set; }
        public long YearlyIncome { get; set; }
        public long YearlyExpenses { get; set; }
    }
}
