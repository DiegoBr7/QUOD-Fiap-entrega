using System;
using Quod.Antifraude.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quod.Antifraude.Core.Repositories
{
    public interface IBiometriaDigitalRepository
    {
        Task RegistrarAsync(BiometriaDigital biometria);
        Task<BiometriaDigital?> ObterPorPessoaIdAsync(Guid pessoaId);
        Task<Pessoa?> ObterPessoaComDigitalPorCpfAsync(string cpf); // Para buscar template junto com Pessoa
    }
}
    

