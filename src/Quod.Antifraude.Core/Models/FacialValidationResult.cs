namespace Quod.Antifraude.Core.Models
{
    public class FacialValidationResult
    {
        public bool EhFraude { get; set; }
        public TipoFraude? TipoFraude { get; set; }
        public double Confidence { get; set; }
        public int FaceCount { get; set; }
        public double LivenessScore { get; set; }
        public string Observacao { get; set; } = string.Empty;

        public DateTime DataAnalise { get; set; } = DateTime.UtcNow;
    }
}
