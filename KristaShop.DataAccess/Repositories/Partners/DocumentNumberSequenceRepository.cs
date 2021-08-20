using System.Threading.Tasks;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class DocumentNumberSequenceRepository : IDocumentNumberSequenceRepository {
        private readonly DbContext _context;

        public DocumentNumberSequenceRepository(DbContext context) {
            _context = context;
        }
        
        public async Task<ulong> NextAsync() {
            if (_context.Database.CurrentTransaction != null) {
                return await _nextAsync();
            }
            
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var result = await _nextAsync();
            await transaction.CommitAsync();
            
            return result;
        }

        private async Task<ulong> _nextAsync() {
            await _context.Database.ExecuteSqlRawAsync("UPDATE part_documents_sequence SET id = LAST_INSERT_ID(id + 1);");
            var result = await _context.Set<ScalarULong>().FromSqlRaw("SELECT LAST_INSERT_ID() AS value FROM part_documents_sequence").FirstAsync();
            return result.Value - 1;
        }
    }
}