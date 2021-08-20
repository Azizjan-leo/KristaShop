using System.Threading.Tasks;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class MoneyDocumentsTotalsRepository : IMoneyDocumentsTotalsRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public MoneyDocumentsTotalsRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }


        public async Task<MoneyDocumentsTotalSqlView> GetUserTotals(int userId) {
            var compiledSql = _compiler.Compile(_createDocumentsTotalsQueryByUser(userId));
            return await _context.Set<MoneyDocumentsTotalSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).FirstOrDefaultAsync();
        }

        private Query _createDocumentsTotalsQuery() {
            return new Query("1c_raschety_klientov AS items")
                .Select("items.id", "items.klient", "items.nach_data", "items.kon_data")
                .SelectRaw("CAST(`items`.`nach_ost` AS DOUBLE) AS `nach_ost`, CAST(`items`.`kon_ost` AS DOUBLE) AS `kon_ost`")
                .SelectRaw("CAST(`items`.`dolg_plus` AS DOUBLE) AS `dolg_plus`, CAST(`items`.`dolg_minus` AS DOUBLE) AS `dolg_minus`");
        }

        private Query _createDocumentsTotalsQueryByUser(int userId) {
            return _createDocumentsTotalsQuery()
                .Where("items.klient", userId)
                .Limit(1);
        }
    }
}