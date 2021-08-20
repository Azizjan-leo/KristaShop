using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IDocumentNumberSequenceRepository : IRepository {
        Task<ulong> NextAsync();
    }
}