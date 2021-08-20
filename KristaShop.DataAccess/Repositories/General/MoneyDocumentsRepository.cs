using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class MoneyDocumentsRepository : IMoneyDocumentsRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public MoneyDocumentsRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<MoneyDocumentSqlView>> GetUserItemsDetailedAsync(int userId) {
            var result = await _getDocumentsAsync(userId);
            var details = (await _getDetailsAsync(userId)).GroupBy(x => x.DocumentId)
                .ToDictionary(k => k.Key, v => v.ToList());
            foreach (var document in result) {
                if (details.ContainsKey(document.Id)) {
                    document.Items = document.Items = details[document.Id];
                }
            }

            return result;
        }
    
        private async Task<IList<MoneyDocumentSqlView>> _getDocumentsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createMoneyDocumentsQueryByUser(userId));
            return await _context.Set<MoneyDocumentSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }
        
        private async Task<IEnumerable<MoneyDocumentItemSqlView>> _getDetailsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createMoneyDocumentsItemsQueryByUser(userId));
            return await _context.Set<MoneyDocumentItemSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createMoneyDocumentsQuery() {
            return new Query("1c_doc_raschety_klientov AS docs")
                .Select("docs.id", "docs.nomer_doc", "docs.name_doc", "docs.klient", "docs.nach_ost", "docs.kon_ost",
                    "docs.dolg_plus", "docs.dolg_minus", "docs.data_doc")
                .OrderByDesc("docs.data_doc");
        }

        private Query _createMoneyDocumentsQueryByUser(int userId) {
            return _createMoneyDocumentsQuery()
                .Where("docs.klient", userId);
        }

        private Query _createMoneyDocumentsItemsQuery() {
            return new Query("1c_doc_raschety_klientov_sostav AS items")
                .Select("items.id", "items.klient", "items.id_doc", "items.model", "models.articul",
                    "models.line AS size_value", "items.color", "colors.name AS color_name",
                    "matcolors.photo AS color_photo", "color_groups.name AS color_group_name",
                    "color_groups.rgb AS color_group_rgb_value", "items.kolvo AS amount",
                    "photos.photo_path AS photo_by_color", "item_descriptor.main_photo")
                .SelectRaw("CAST(`items`.`price` AS DECIMAL) AS `price`, 0.0 AS `price_rub`")
                .LeftJoin("1c_doc_raschety_klientov AS docs", "docs.id", "items.id_doc")
                .Join("1c_models AS models", "items.model", "models.id")
                .LeftJoin("1c_material AS materials", "materials.id", "models.material")
                .Join("1c_colors AS colors", "colors.id", "items.color")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "items.color").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS item_descriptor", "models.articul", "item_descriptor.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"),
                    x => x.On("photos.articul", "models.articul").On("photos.color_id", "items.color"));
        }

        private Query _createMoneyDocumentsItemsQueryByUser(int userId) {
            return _createMoneyDocumentsItemsQuery()
                .Where("items.klient", userId);
        }
    }
}