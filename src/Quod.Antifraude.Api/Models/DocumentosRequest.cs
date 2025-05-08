// src/Quod.Antifraude.Api/Models/DocumentosRequest.cs
using Microsoft.AspNetCore.Http;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Api.Models
{
    public class DocumentosRequest : BaseCapturaDto
    {
        public required IFormFile Documento { get; set; }
        public required IFormFile ImagemFace { get; set; }
    }
}
