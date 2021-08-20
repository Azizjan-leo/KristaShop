using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class OrderTotalsRepository : IOrderTotalsRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public OrderTotalsRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<OrderTotalsSqlView>> GetUserTotalsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createUserTotalsQuery(userId));
            return await _context.Set<OrderTotalsSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private static Query _createUserTotalsQuery(int userId) {
            return _createUserRequestsTotalsQuery(userId)
                .Union(_createUserManufactureTotals(userId))
                .Union(_createUserReservationsTotals(userId))
                .Union(_createUserBalanceQuery(userId));
        }

        private static Query _createUserRequestsTotalsQuery(int userId) {
            return new Query("1c_zayavka_klientov AS items")
                .Join("1c_models AS models", "items.model", "models.id")
                .LeftJoin("1c_collection AS collection", $"models.collection", "collection.id")
                .SelectRaw("ifnull(sum(`items`.`cena` * `items`.`kolichestvo`), 0) AS `sum`")
                .SelectRaw("ifnull(sum(`items`.`cena_rub` * `items`.`kolichestvo`), 0) AS `sum_rub`")
                .SelectRaw("ifnull(AVG(IF(`collection`.`procent` IS NULL OR `collection`.`procent` = 0, ?, `collection`.`procent`)), ?) AS `prepay_percent`", GlobalConstant.GeneralPrepayPercentValue, GlobalConstant.GeneralPrepayPercentValue)
                .SelectRaw("? AS `type`", (int)OrderTotalsReportType.Request)
                .Where("items.klient", userId);
        }

        private static Query _createUserManufactureTotals(int userId) {
            return new Query("1c_klient_proizvodstvo AS items")
                .Join("1c_models AS models", "items.model", "models.id")
                .LeftJoin("1c_collection AS collection", $"models.collection", "collection.id")
                .SelectRaw("ifnull(sum(`items`.`cena` * (`items`.`kroitsya` + `items`.`gotovkroy` + `items`.`zapusk` + `items`.`vposhive` + `items`.`skladgp`)), 0) AS `sum`")
                .SelectRaw("ifnull(sum(`items`.`cena_rub` * (`items`.`kroitsya` + `items`.`gotovkroy` + `items`.`zapusk` + `items`.`vposhive` + `items`.`skladgp`)), 0) AS `sum_rub`")
                .SelectRaw("ifnull(AVG(IF(`collection`.`procent` IS NULL OR `collection`.`procent` = 0, ?, `collection`.`procent`)), ?) AS `prepay_percent`", GlobalConstant.GeneralPrepayPercentValue, GlobalConstant.GeneralPrepayPercentValue)
                .SelectRaw("? AS `type`", (int)OrderTotalsReportType.Manufacture)
                .Where("items.klient", userId);
        }

        private static Query _createUserReservationsTotals(int userId) {
            return new Query("1c_rezervy_klientov as items")
                .Join("1c_models AS models", "items.model", "models.id")
                .LeftJoin("1c_collection AS collection", $"models.collection", "collection.id")
                .SelectRaw("ifnull(sum(`items`.`cena` * `items`.`kolichestvo`), 0) AS `sum`")
                .SelectRaw("ifnull(sum(`items`.`cena_rub` * `items`.`kolichestvo`), 0) AS `sum_rub`")
                .SelectRaw("ifnull(AVG(IF(`collection`.`procent` IS NULL OR `collection`.`procent` = 0, ?, `collection`.`procent`)), ?) AS `prepay_percent`", GlobalConstant.GeneralPrepayPercentValue, GlobalConstant.GeneralPrepayPercentValue)
                .SelectRaw("? AS `type`", (int)OrderTotalsReportType.Reservation)
                .Where("items.klient", userId);
        }

        private static Query _createUserBalanceQuery(int userId) {
            return new Query("1c_clients AS client")
                .SelectRaw("`client`.`avansdolg` AS `sum`, `client`.`avansdolgrub` AS `sum_rub`")
                .SelectRaw("? AS `type`", (int)OrderTotalsReportType.Balance)
                .SelectRaw("0 AS `prepay_percent`")
                .Where("id", userId);
        }
    }
}
