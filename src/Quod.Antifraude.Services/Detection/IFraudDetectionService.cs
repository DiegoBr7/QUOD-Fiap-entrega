// src/Quod.Antifraude.Services/Detection/IFraudDetectionService.cs
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Services.Detection
{
    public interface IFraudDetectionService
    {
        Task<(bool ehFraude, TipoFraude? tipoFraude)> DetectAsync(IFormFile imagem);
        Task<(bool ehFraude, TipoFraude? tipoFraude)> DetectDocumentosAsync(Stream documento, Stream selfie);
    }
}
