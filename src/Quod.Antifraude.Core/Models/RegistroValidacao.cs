namespace Quod.Antifraude.Core.Models
{
    public class RegistroValidacao : BaseCapturaDto
    {
        public bool EhFraude { get; set; }
        public TipoFraude? TipoFraude { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}
