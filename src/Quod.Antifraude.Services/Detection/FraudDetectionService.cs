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
        private static readonly string[] _permitidos = { "image/png", "image/jpeg" };
        private readonly Random _rnd = new Random();

        // Fluxo público para validação facial
        public async Task<FacialValidationResult> ValidateFacialAsync(IFormFile imagem)
        {
            ValidateImage(imagem);

            bool isFraud = ShouldSimulateFraud(0.1); // 10% de chance
            return isFraud
                ? BuildFraudResult(TipoFraude.Deepfake)
                : BuildSuccessResult();
        }

        // Fluxo público para validação de digitais
        public async Task<FacialValidationResult> ValidateDigitalAsync(IFormFile imagemDigital)
        {
            ValidateImage(imagemDigital);

            bool isFraud = ShouldSimulateFraud(0.1); // 10% de chance
            return isFraud
                ? BuildFraudResult(TipoFraude.Deepfake)
                : BuildSuccessResult();
        }

        // Fluxo público para comparação de documento + selfie
        public async Task<(bool EhFraude, TipoFraude? TipoFraude)> DetectDocumentosAsync(
            Stream doc, Stream selfie)
        {
            bool isFraud = ShouldSimulateFraud(0.2); // 20% de chance
            return (isFraud, isFraud ? TipoFraude.Mascara : null);
        }

        // === Métodos auxiliares privados ===

        // Valida tamanho e formato do arquivo
        private void ValidateImage(IFormFile imagem)
        {
            if (imagem is null || imagem.Length == 0 || imagem.Length > 5 * 1024 * 1024)
                throw new InvalidDataException("Arquivo inválido ou muito grande.");
            if (!_permitidos.Contains(imagem.ContentType))
                throw new InvalidDataException($"Formato não suportado ({imagem.ContentType}).");
        }

        // Decide, de forma aleatória, se vai simular fraude
        private bool ShouldSimulateFraud(double probability)
        {
            return _rnd.NextDouble() < probability;
        }

        // Constrói o resultado de sucesso (sem fraude)
        private FacialValidationResult BuildSuccessResult()
        {
            return new FacialValidationResult
            {
                EhFraude = false,
                TipoFraude = null,
                Confidence = 1.0,      // 100% de confiança para sucesso
                FaceCount = 1,        // pelo menos uma face detectada
                LivenessScore = 1.0,      // liveness perfeito
                Observacao = "OK"
            };
        }

        // Constrói o resultado de fraude, recebendo o tipo
        private FacialValidationResult BuildFraudResult(TipoFraude tipo)
        {
            return new FacialValidationResult
            {
                EhFraude = true,
                TipoFraude = tipo,
                Confidence = _rnd.NextDouble(),
                FaceCount = _rnd.Next(1, 3),
                LivenessScore = _rnd.NextDouble(),
                Observacao = tipo == TipoFraude.Deepfake
                                  ? "Deepfake simulado"
                                  : "Máscara/documento falsificado"
            };
        }
    }
}
