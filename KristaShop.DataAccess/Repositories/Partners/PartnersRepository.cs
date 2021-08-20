using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Filters;
using System.Linq;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class PartnersRepository : Repository<Partner, int>, IPartnersRepository {
        private readonly MySqlCompiler _compiler;

        public PartnersRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public async Task<List<DocumentItem>> GetDecryptedSalesReportItems(ReportsFilter filter) {
            var sourceItems = _applyReportsFilter(filter);
            var resultItems = sourceItems.Where(x => x.Document.DocumentType == nameof(SellingDocument))
               .GroupBy(x => new {
                   x.ModelId,
                   x.ColorId,
                   ColorName = x.Color.Name,
                   ColorCode = x.Color.Group != null ? x.Color.Group.Hex : "",
                   x.Model.SizeLine,
                   x.Size,
                   x.Articul,
                   x.Model.Name,
                   x.Document.Partner.PaymentRate,
                   x.Model.Descriptor.MainPhoto
               })
               .Select(x => new DocumentItem {
                   ModelId = x.Key.ModelId,
                   ColorId = x.Key.ColorId,
                   Articul = x.Key.Articul,
                   Amount = x.Sum(c => c.Amount),
                   Color = new Color {
                       Id = x.Key.ColorId,
                       Name = x.Key.ColorName,
                       Group = new ColorGroup { Hex = x.Key.ColorCode }
                   },
                   Model = new Model {
                       Id = x.Key.ModelId,
                       Articul = x.Key.Articul,
                       Name = x.Key.Name,
                       Descriptor = new CatalogItemDescriptor() {
                           MainPhoto = x.Key.MainPhoto
                       }
                   },
                   Size = new Common.Models.Structs.SizeValue(x.Key.Size.Value, x.Key.SizeLine),
                   Price = x.Sum(c => c.Amount * x.Key.PaymentRate)
                   
               });
            return await resultItems.ToListAsync();
        }

        public async Task<List<PartnerSalesReportItem>> GetSalesReportItems(ReportsFilter filter) {
            var sourceItems = _applyReportsFilter(filter);

            var resultItems = sourceItems.Where(x => x.Document.DocumentType == nameof(SellingDocument))
               .GroupBy(x => new {
                   x.Document.UserId,
                   UserFullName = x.Document.Partner.User.FullName,
                   x.Document.Partner.User.ManagerId,
                   x.Document.Partner.User.CityId,
                   CityName = x.Document.Partner.User.City.Name,
                   ManagerName = x.Document.Partner.User.Manager.Name,
                   x.Document.Partner.User.MallAddress,
                   x.Document.Partner.PaymentRate
               })
               .Select(x => new PartnerSalesReportItem {
                   UserId = x.Key.UserId,
                   UserFullName = x.Key.UserFullName,
                   ManagerId = x.Key.ManagerId ?? 0,
                   ManagerName = x.Key.ManagerName,
                   CityId = x.Key.CityId ?? 0,
                   CityName = x.Key.CityName,
                   MallAddress = x.Key.MallAddress,
                   Amount = x.Sum(c => c.Amount),
                   Sum = x.Sum(c => c.Amount * x.Key.PaymentRate)
               });
            return await resultItems.ToListAsync();
        }


        public async Task<List<PartnerSqlView>> GetAllPartnersAsync(PartnersFilter filter) {
            var compiledSql = _compiler.Compile(_createFilterPartnerSqlViewQuery(_createPartnerSqlViewQuery(), filter));
            return await Context.Set<PartnerSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }
        
        public async Task<List<LookUpItem<int, string>>> GetPartnersLookUpAsync() {
            return await All
                .Select(x => new LookUpItem<int, string>(x.UserId, x.User.FullName))
                .ToListAsync();
        }

        public async Task<double> GetPartnerPaymentRateAsync(int userId) {
            return await All.Where(x => x.UserId == userId)
                .Select(x => x.PaymentRate)
                .FirstAsync();
        }

        public async Task<bool> IsPartnerAsync(int userId) {
            return await All.AnyAsync(x => x.UserId == userId);
        }

        private Query _createPartnerSqlViewQuery() {
            return new Query("partners")
                .Select("partners.user_id")
                .Select("client.fullname", "client.addresstc", "client.city", "client.phone", "client.email", "client.login")
                .SelectRaw("IFNULL(`shipments`.`shipments_items_sum`, 0.0) AS `shipments_items_sum`, IFNULL(`shipments`.`shipments_items_count`, 0) AS `shipments_items_count`")
                .SelectRaw("IFNULL(`storehouse_items`.`storehouse_items_count`, 0) AS storehouse_items_count")
                .SelectRaw("IFNULL(`manager`.`name`, '') AS `manager_name`")
                .SelectRaw("IFNULL(`1c_city`.`name`, '') AS `city_name`")
                .SelectRaw("IFNULL(payment_totals.total_amount, 0) AS debt_items_count")
                .SelectRaw("IFNULL(payment_totals.total_sum, 0.0) AS debt_items_sum")
                .SelectRaw("last_revision.revision_date AS revision_date")
                .SelectRaw("last_payments.payment_date")
                .Join("1c_clients AS client", "client.id", "partners.user_id")
                .LeftJoin("1c_manager AS manager", "manager.id", "client.manager")
                .LeftJoin("1c_city", "1c_city.id", "client.city")
                .LeftJoin(_createUserShipmentsTotalQuery().As("shipments"), x => x.On("shipments.klient", "client.id"))
                .LeftJoin(_createUserStorehouseItemsTotalQuery().As("storehouse_items"), x => x.On("storehouse_items.user_id", "client.id"))
                .LeftJoin(_createUserLastPaymentQuery().As("last_payments"), x => x.On("last_payments.user_id", "client.id"))
                .LeftJoin(_createUserLastRevisionQuery().As("last_revision"), x => x.On("last_revision.user_id", "client.id"))
                .LeftJoin(_createUserToPayTotalsQuery().As("payment_totals"), x => x.On("payment_totals.user_id", "client.id"));
        }

        private Query _createFilterPartnerSqlViewQuery(Query query, PartnersFilter filter) {
            if (filter.Cities.Any()) {
                query = query.WhereIn("client.city", filter.Cities);
            }

            if (filter.Partners.Any()) {
                query = query.WhereIn("client.id", filter.Partners);
            }
            
            if (filter.Managers.Any()) {
                query = query.WhereIn("client.manager", filter.Managers);
            }

            if (filter.PaymentDate.From != null) {
                query = query.Where("last_payments.payment_date", ">=", filter.PaymentDate.From);
            }
            
            if (filter.PaymentDate.To != null) {
                query = query.Where("last_payments.payment_date", "<=", filter.PaymentDate.To);
            }
            
            if (filter.RevisionDate.From != null) {
                query = query.Where("last_revision.revision_date", ">=", filter.RevisionDate.From);
            }
            
            if (filter.RevisionDate.To != null) {
                query = query.Where("last_revision.revision_date", "<=", filter.RevisionDate.To);
            }

            return query;
        }

        private static Query _createUserStorehouseItemsTotalQuery() {
            return new Query("part_storehouse_items AS items")
                .Select("items.user_id")
                .SelectRaw("SUM(`items`.`amount`) AS `storehouse_items_count`")
                .GroupBy("items.user_id");
        }

        private static Query _createUserShipmentsTotalQuery() {
            return new Query("1c_prodagi_klientov AS shipments")
                .Select("shipments.klient")
                .SelectRaw("SUM(`shipments`.`cena` * `shipments`.`kolichestvo`) AS `shipments_items_sum`")
                .SelectRaw("SUM(`shipments`.`kolichestvo`) AS `shipments_items_count`")
                .GroupBy("shipments.klient");
        }

        private static Query _createUserLastPaymentQuery() {
            return new Query("part_documents AS docs")
                .Select("docs.user_id")
                .SelectRaw("MAX(`docs`.`create_date`) AS payment_date")
                .Where("document_type", nameof(PaymentDocument))
                .Where("state", State.Paid.ToString())
                .GroupBy("docs.user_id");
        }

        private static Query _createUserLastRevisionQuery() {
            return new Query("part_documents AS docs")
                .Select("docs.user_id")
                .SelectRaw("MAX(`docs`.`create_date`) AS revision_date")
                .Where("document_type", nameof(RevisionDocument))
                .GroupBy("docs.user_id");
        }

        private static Query _createUserToPayTotalsQuery() {
            return new Query("part_documents AS docs")
                .Select("docs.user_id")
                .SelectRaw("SUM(`items`.`amount`) AS total_amount")
                .SelectRaw("SUM(`items`.`price` * `items`.`amount`) AS total_sum")
                .LeftJoin("part_documents_items AS items", "items.document_id", "docs.id")
                .Where("document_type", nameof(PaymentDocument))
                .WhereNot("state", State.Paid.ToString())
                .GroupBy("docs.user_id");
        }

        private IQueryable<DocumentItem> _applyReportsFilter(ReportsFilter filter) {
            var sourceItems = Context.Set<DocumentItem>().AsQueryable();

            if (filter.SelectedArticuls?.Any() is true) {
                sourceItems = sourceItems.Where(x => filter.SelectedArticuls.Contains(x.Articul));
            }

            if (filter.SelectedColorIds?.Any() is true) {
                sourceItems = sourceItems.Where(x => filter.SelectedColorIds.Contains(x.ColorId));
            }

            if (filter.SelectedCityIds?.Any() is true) {

                sourceItems = sourceItems.Where(x => filter.SelectedCityIds.Contains(x.Document.Partner.User.CityId.Value));
            }
            if (filter.SelectedUserIds?.Any() is true) {
                sourceItems = sourceItems.Where(x => filter.SelectedUserIds.Contains(x.Document.UserId));
            }

            if (filter.SelectedManagerIds?.Any() is true) {
                sourceItems = sourceItems.Where(x => filter.SelectedManagerIds.Contains(x.Document.Partner.User.ManagerId.Value));
            }

            if (filter.Date.From != null) {
                sourceItems = sourceItems.Where(x => x.Date >= filter.Date.From);
            }

            if (filter.Date.To != null) {
                sourceItems = sourceItems.Where(x => x.Date <= filter.Date.To);
            }
            return sourceItems;
        }
    }
}
