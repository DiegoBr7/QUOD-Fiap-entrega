namespace Quod.Antifraude.Core.Models
{
    public class BaseCapturaDto
    {
        public required Guid TransacaoId { get; set; }
        public required TipoBiometria TipoBiometria { get; set; }
        public required DateTime DataCaptura { get; set; }
        public required DeviceInfo Dispositivo { get; set; }
        public required LocationInfo MetadadosLocalizacao { get; set; }
    }
}
