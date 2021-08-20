using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class RequestItemsRepository : IRequestItemsRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public RequestItemsRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<ClientRequestItemSqlView>> GetUserItemsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createRequestItemsQueryByUserId(userId));
            return await _context.Set<ClientRequestItemSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createRequestItemsQuery() {
            return new Query("1c_zayavka_klientov AS request")
                .Select("request.id", "request.klient AS user_id", "models.articul", "request.model AS model_id",
                    "request.color AS color_id", "models.line AS size_value", "request.kolichestvo AS amount",
                    "request.datav AS request_date", "request.cena", "request.cena_rub",
                    "colors.name AS color_name", "matcolors.photo AS color_photo", "color_groups.name AS color_group_name",
                    "color_groups.rgb AS color_group_rgb_value", "photos.photo_path AS photo_by_color", "item_descriptor.main_photo")
                .Join("1c_models AS models", "request.model", "models.id")
                .LeftJoin("1c_material AS materials", "models.material", "materials.id")
                .Join("1c_colors AS colors", "request.color", "colors.id")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "request.color").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS item_descriptor", "models.articul", "item_descriptor.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"),
                    x => x.On("photos.articul", "models.articul").On("photos.color_id", "request.color"))
                .JoinModelCollection("models")
                .OrderBy("request.datav", "models.articul", "models.line", "colors.name");
        }

        private Query _createRequestItemsQueryByUserId(int userId) {
            return _createRequestItemsQuery()
                .Where("request.klient", userId);
        }
    }
}
