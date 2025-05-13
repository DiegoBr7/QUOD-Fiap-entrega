using System.Security.Cryptography;
using System.Text;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Repositories;

namespace Quod.Antifraude.Services.Biometria
{
    public class BiometriaDigitalService    : IBiometriaDigitalService
    {
        private readonly IBiometriaDigitalRepository _biometriaRepo;

        public BiometriaDigitalService(IBiometriaDigitalRepository biometriaRepo)
        {
            _biometriaRepo = biometriaRepo;
        }

        public Task<string> SimularCapturaDigitalAsync()
        {
            // Simulação: Gera um template aleatório
            string dadosSimulados = $"DigitalCapturada_{Guid.NewGuid()}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(dadosSimulados));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return Task.FromResult(builder.ToString());
            }
        }

        public async Task<bool> RegistrarDigitalAsync(Guid pessoaId, string templateDigital)
        {
            // Verifica se já existe um template para essa pessoa

            var biometria = new BiometriaDigital
            {
                PessoaId = pessoaId,
                TemplateDigital = templateDigital
            };
            await _biometriaRepo.RegistrarAsync(biometria);
            return true;
        }

        public async Task<(bool Sucesso, string Mensagem, double? Similaridade)> ValidarDigitalAsync(string cpf, string templateCapturado)
        {
            var pessoaComDigital = await _biometriaRepo.ObterPessoaComDigitalPorCpfAsync(cpf);

            if (pessoaComDigital == null || string.IsNullOrEmpty(pessoaComDigital.TemplateBiometriaDigital))
            {
                return (false, "Biometria digital não registrada para este CPF.", null);
            }

            // Simulação de Validação: Compara os templates
            bool corresponde = pessoaComDigital.TemplateBiometriaDigital.Equals(templateCapturado, StringComparison.OrdinalIgnoreCase);

            if (corresponde)
            {
                //  simulação, podemos retornar 100% de similaridade se os hashes baterem.
                return (true, "Biometria digital validada com sucesso.", 1.0);
            }
            else
            {
                //  simulação, 0% se não bater.
                return (false, "Biometria digital não corresponde.", 0.0);
            }
        }
    }
}
