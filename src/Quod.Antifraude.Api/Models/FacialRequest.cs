using Microsoft.AspNetCore.Http;
using Quod.Antifraude.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Quod.Antifraude.Api.Models
{
    /// <summary>
    /// DTO para captura de imagem facial.
    /// </summary>
    public class FacialRequest : BaseCapturaDto
    {
        [SwaggerSchema("Imagem capturada pela câmera frontal (PNG ou JPEG, ≤ 5MB)")]
        public required IFormFile Imagem { get; set; }
    }
}
