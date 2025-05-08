// src/Quod.Antifraude.Api/Models/ValidacaoRequest.cs
using Microsoft.AspNetCore.Http;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Api.Models
{
    public class ValidacaoRequest : BaseCapturaDto
    {
        public required IFormFile Imagem { get; set; }
    }
}
