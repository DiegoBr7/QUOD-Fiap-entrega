// src/Quod.Antifraude.Services/Detection/IFraudDetectionService.cs
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Services.Detection
{
    public interface IFraudDetectionService
    {
        Task<FacialValidationResult> ValidateFacialAsync(IFormFile imagem);

        // Novo método para digitais:
        Task<FacialValidationResult> ValidateDigitalAsync(IFormFile imagemDigital);

        Task<(bool EhFraude, TipoFraude? TipoFraude)> DetectDocumentosAsync(
            Stream doc,
            Stream selfie);
    }
}
