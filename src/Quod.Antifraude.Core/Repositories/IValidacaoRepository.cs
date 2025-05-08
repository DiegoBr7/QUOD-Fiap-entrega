using System.Threading.Tasks;
using Quod.Antifraude.Core.Models;

namespace Quod.Antifraude.Core.Repositories
{
    public interface IValidacaoRepository
    {
        Task SaveAsync(RegistroValidacao registro);
    }
}
