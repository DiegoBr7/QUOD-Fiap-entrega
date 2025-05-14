using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Settings;

namespace Quod.Antifraude.Infrastructure.Repositories
{
    public class BiometriaDigitalRepository
    {
        private readonly IMongoCollection<BiometriaDigital> _biometriaCollection;
        private readonly IMongoCollection<Pessoa> _pessoaCollection; // Adicionar se for atualizar Pessoa

        public BiometriaDigitalRepository(IOptions<MongoSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _biometriaCollection = mongoDatabase.GetCollection<BiometriaDigital>("BiometriasDigitais");
            _pessoaCollection = mongoDatabase.GetCollection<Pessoa>("Pessoas"); // Coleção de Pessoas
        }

        public async Task RegistrarAsync(BiometriaDigital biometria)
        {
            // Opção 1: Coleção separada
            // Opção 2: Atualizar documento Pessoa 
            var filter = Builders<Pessoa>.Filter.Eq(p => p.Id, biometria.PessoaId.ToString());
            var update = Builders<Pessoa>.Update.Set(p => p.TemplateBiometriaDigital, biometria.TemplateDigital);
            await _pessoaCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
        }

        public async Task<BiometriaDigital?> ObterPorPessoaIdAsync(Guid pessoaId)
        {

            // Se template estiver em Pessoa:
            var pessoa = await _pessoaCollection.Find(p => p.Id == pessoaId.ToString()).FirstOrDefaultAsync();
            if (pessoa?.TemplateBiometriaDigital != null)
            {
                return new BiometriaDigital
                {
                    PessoaId = pessoaId,
                    TemplateDigital = pessoa.TemplateBiometriaDigital,
                };
            }
            return null;
        }
        public async Task<Pessoa?> ObterPessoaComDigitalPorCpfAsync(string cpf)
        {
            return await _pessoaCollection.Find(p => p.Cpf == cpf).FirstOrDefaultAsync();
        }
    }
}

