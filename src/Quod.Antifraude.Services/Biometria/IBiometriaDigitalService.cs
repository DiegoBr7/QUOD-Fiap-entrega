namespace Quod.Antifraude.Services.Biometria
{
     public interface IBiometriaDigitalService
    {
        Task<string> SimularCapturaDigitalAsync(); // Retorna um "template" simulado
        Task<bool> RegistrarDigitalAsync(Guid pessoaId, string templateDigital);
        Task<(bool Sucesso, string Mensagem, double? Similaridade)> ValidarDigitalAsync(string cpf, string templateCapturado);
    }
}

