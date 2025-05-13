using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Quod.Antifraude.Core.Models
{
    public class BiometriaDigital
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Guid PessoaId { get; set; } // ID da pessoa a quem a digital pertence

        public string? TemplateDigital { get; set; } // Representação do template da digital

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}

