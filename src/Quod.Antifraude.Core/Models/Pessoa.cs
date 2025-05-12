using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Quod.Antifraude.Core.Models
{
    public class Pessoa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("cpf")]
        public string Cpf { get; set; } = null!;
    }
}
