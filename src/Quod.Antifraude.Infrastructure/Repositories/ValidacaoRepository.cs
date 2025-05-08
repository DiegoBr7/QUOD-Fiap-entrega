using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quod.Antifraude.Core.Models;
using Quod.Antifraude.Core.Repositories;
using Quod.Antifraude.Core.Settings;

namespace Quod.Antifraude.Infrastructure.Repositories
{
    public class ValidacaoRepository : IValidacaoRepository
    {
        private readonly IMongoCollection<RegistroValidacao> _collection;

        public ValidacaoRepository(IOptions<MongoSettings> opts)
        {
            var client = new MongoClient(opts.Value.ConnectionString);
            var db = client.GetDatabase(opts.Value.DatabaseName);
            _collection = db.GetCollection<RegistroValidacao>("validacoes");
        }

        public Task SaveAsync(RegistroValidacao registro) =>
            _collection.InsertOneAsync(registro);
    }
}
