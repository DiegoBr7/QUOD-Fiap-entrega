using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Core.Repositories
{
    public interface IBiometriaDigitalRepository
    {
        Task RegistrarAsync(BiometriaDigital biometria);
        Task<BiometriaDigital?> ObterPorPessoaIdAsync(Guid pessoaId);
        Task<Pessoa?> ObterPessoaComDigitalPorCpfAsync(string cpf); // Para buscar template junto com Pessoa
    }
}


