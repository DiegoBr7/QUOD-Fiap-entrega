// src/Quod.Antifraude.Services/Detection/FraudDetectionService.cs
using Microsoft.AspNetCore.Http;
using Quod.Antifraude.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quod.Antifraude.Services.Detection
{
    public class FraudDetectionService : IFraudDetectionService
    {
        public async Task<(bool ehFraude, TipoFraude? tipoFraude)> DetectAsync(IFormFile imagem)
        {
            // validações básicas
            if (imagem is null || imagem.Length == 0 || imagem.Length > 5 * 1024 * 1024)
                throw new InvalidDataException("Arquivo inválido ou muito grande");
            var permitidos = new[] { "image/png", "image/jpeg" };
            if (!permitidos.Contains(imagem.ContentType))
                throw new InvalidDataException($"Formato não suportado ({imagem.ContentType})");

            // simula fraude
            using var stream = imagem.OpenReadStream();
            var rnd = new Random();
            bool isFraud = rnd.NextDouble() < 0.1;
            TipoFraude? tipo = isFraud ? TipoFraude.Deepfake : null;
            return (isFraud, tipo);
        }

        public async Task<(bool ehFraude, TipoFraude? tipoFraude)> DetectDocumentosAsync(
            Stream documento, Stream selfie)
        {
            // aqui poderia ter lógica diferente: checar dois streams, comparar faces, etc.
            var rnd = new Random();
            bool isFraud = rnd.NextDouble() < 0.2; // 20% de chance
            TipoFraude? tipo = isFraud ? TipoFraude.Mascara : null;
            return (isFraud, tipo);
        }
    }
}
