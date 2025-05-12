// src/Quod.Antifraude.Api/Models/FacialValidationResponse.cs
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Api.Models
{
    /// <summary>
    /// Payload retornado pelo endpoint de validação facial.
    /// </summary>
    public class FacialValidationResponse
    {
        public Guid TransacaoId { get; set; }
        public bool EhFraude { get; set; }
        public TipoFraude? TipoFraude { get; set; }
        public double Confidence { get; set; }
        public int FaceCount { get; set; }
        public double LivenessScore { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}
