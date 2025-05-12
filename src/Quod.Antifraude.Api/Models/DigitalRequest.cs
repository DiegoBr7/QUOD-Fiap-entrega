// src/Quod.Antifraude.Api/Models/DigitalRequest.cs
using Microsoft.AspNetCore.Http;
using Quod.Antifraude.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Quod.Antifraude.Api.Models
{
    public class DigitalRequest : BaseCapturaDto
    {
        [SwaggerSchema(Description = "Scan da impressão digital para verificação de fraude")]
        public required IFormFile ImagemDigital { get; set; }

        // Não é necessário redeclarar TransacaoId, TipoBiometria, etc.
    }
}
