using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class ReservationItemsRepository : IReservationItemsRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public ReservationItemsRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<ClientReservationItemSqlView>> GetUserItemsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createReservationItemsQueryByUserId(userId));
            return await _context.Set<ClientReservationItemSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createReservationItemsQuery() {
            return new Query("1c_rezervy_klientov AS items")
                .Select("items.id", "items.klient AS user_id", "models.articul", "items.model AS model_id",
                    "items.color AS color_id", "items.ves", "items.kolichestvo AS amount",
                    "items.datav AS reservation_date", "items.cena", "items.cena_rub", "colors.name AS color_name",
                    "matcolors.photo AS color_photo", "color_groups.name AS color_group_name",
                    "color_groups.rgb AS color_group_rgb_value",
                    "photos.photo_path AS photo_by_color", "item_descriptor.main_photo")
                .SelectRaw("if(`items`.`razmer` = '', `models`.`line`, TRIM(`items`.`razmer`)) AS `size_value`")
                .Join("1c_models AS models", "items.model", "models.id")
                .LeftJoin("1c_material AS materials", "models.material", "materials.id")
                .Join("1c_colors AS colors", "items.color", "colors.id")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "items.color").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS item_descriptor", "models.articul", "item_descriptor.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"),
                    x => x.On("photos.articul", "models.articul").On("photos.color_id", "items.color"))
                .JoinModelCollection("items")
                .OrderBy("items.datav", "models.articul", "models.line", "size_value", "colors.name");
        }

        private Query _createReservationItemsQueryByUserId(int userId) {
            return _createReservationItemsQuery()
                .Where("items.klient", userId);
        }
    }
}