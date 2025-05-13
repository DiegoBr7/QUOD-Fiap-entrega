using System;
using Quod.Antifraude.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quod.Antifraude.Services.Biometria
{
     public interface IBiometriaDigitalService
    {
        Task<string> SimularCapturaDigitalAsync(); // Retorna um "template" simulado
        Task<bool> RegistrarDigitalAsync(Guid pessoaId, string templateDigital);
        Task<(bool Sucesso, string Mensagem, double? Similaridade)> ValidarDigitalAsync(string cpf, string templateCapturado);
    }
}

