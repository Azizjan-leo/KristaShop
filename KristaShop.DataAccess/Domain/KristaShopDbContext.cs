using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Configurations;
using KristaShop.DataAccess.Configurations.DataFor1C;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Configurations.Partners;
using KristaShop.DataAccess.Configurations.Views;
using KristaShop.DataAccess.Domain.Seeds;
using KristaShop.DataAccess.Domain.Seeds.Common;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Domain {
    public class KristaShopDbContext : ChangeLogDbContext {

        public KristaShopDbContext (DbContextOptions<KristaShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.InitializeMysqlCustomFunctions();

            builder.ApplyConfiguration(new ManagerDetailsConfiguration());
            builder.ApplyConfiguration(new ManagerConfiguration());
            builder.ApplyConfiguration(new ManagerDetailsSqlViewConfiguration());
            builder.ApplyConfiguration(new ManagerAccessConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RoleAccessConfiguration());
            builder.ApplyConfiguration(new DataChangeLogConfiguration());

            ApplyBaseEntitiesConfiguration(builder);
            Apply1CEntitiesConfiguration(builder);
            ApplyViewsConfiguration(builder);
            ApplyPartnersConfiguration(builder);


            builder.Seed(new CatalogsSeed());
            //builder.CategoryBuilder();
            builder.Seed(new MenuItemsSeed());
            builder.Seed(new SettingsSeed());
            builder.Seed(new SequencesSeed());
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<AppliedImport> AppliedImports { get; set; }
        public DbSet<PromoLink> PromoLinks { get; set; }
        public DbSet<AppliedImportItem> AppliedImportItems { get; set; }

        #region 1C Web Shop Modifications
        public DbSet<ModelCatalogOrder> ModelCatalogOrder { get; set; }
        public DbSet<CatalogItemDescriptor> CatalogItemDescriptor { get; set; }
        public DbSet<Order> Orders1C { get; set; }
        public DbSet<OrderDetails> Order1CDetails { get; set; }
        public DbSet<OrderDetailsFull> OrderDetailsFull { get; set; }
        public DbSet<Favorite1CItemItem> Favorite1CItemItems { get; set; }
        #endregion

        private static void ApplyPartnersConfiguration(ModelBuilder builder) {
            builder.ApplyConfiguration(new PartnerExcessAndDeficiencyHistoryItemConfiguration());
            builder.ApplyConfiguration(new PartnerStorehouseItemConfiguration());
            builder.ApplyConfiguration(new PartnerStorehouseHistoryItemConfiguration());
            builder.ApplyConfiguration(new PartnerStorehouseHistoryItemSqlViewConfiguration());
            builder.ApplyConfiguration(new PartnerStorehouseItemSqlViewConfiguration());
            builder.ApplyConfiguration(new PartnershipRequestConfiguration());
            builder.ApplyConfiguration(new PartnershipRequestSqlViewConfiguration());
            builder.ApplyConfiguration(new PartnersSqlViewConfiguration());
            builder.ApplyConfiguration(new PartnerConfiguration());
            ApplyPartnersDocumentsConfiguration(builder);
        }

        private static void ApplyPartnersDocumentsConfiguration(ModelBuilder builder) {
            builder.ApplyConfiguration(new DocumentConfiguration());
            builder.ApplyConfiguration(new StorehouseDocumentConfiguration());
            builder.ApplyConfiguration(new IncomeDocumentConfiguration());
            builder.ApplyConfiguration(new SellingDocumentConfiguration());
            builder.ApplyConfiguration(new RevisionDocumentConfiguration());
            builder.ApplyConfiguration(new RevisionDeficiencyDocumentConfiguration());
            builder.ApplyConfiguration(new RevisionExcessDocumentConfiguration());
            builder.ApplyConfiguration(new MoneyDocumentConfiguration());
            builder.ApplyConfiguration(new PaymentDocumentConfiguration());
            builder.ApplyConfiguration(new SellingRequestDocumentConfiguration());
            builder.ApplyConfiguration(new DocumentItemsConfiguration());
            builder.ApplyConfiguration(new DocumentNumberSequenceConfiguration());
        }

        private static void ApplyViewsConfiguration(ModelBuilder builder) {
            builder.ApplyConfiguration(new AppliedImportItemConfiguration());
            builder.ApplyConfiguration(new CartTotalsConfiguration());
            builder.ApplyConfiguration(new ReportTotalsConfiguration());
            builder.ApplyConfiguration(new CreateTableStatementConfiguration());
            builder.ApplyConfiguration(new ScalarConfiguration());
            builder.ApplyConfiguration(new ScalarIntConfiguration());
            builder.ApplyConfiguration(new ScalarLongConfiguration());
            builder.ApplyConfiguration(new LookupListItemConfiguration());
            builder.ApplyConfiguration(new OrderTotalsSqlViewConfiguration());
        }

        private static void ApplyBaseEntitiesConfiguration(ModelBuilder builder) {
            builder.ApplyConfiguration(new AuthorizationLinkConfiguration());
            builder.ApplyConfiguration(new DynamicPageConfiguration());
            builder.ApplyConfiguration(new MenuItemConfiguration());
            builder.ApplyConfiguration(new BlogItemConfiguration());
            builder.ApplyConfiguration(new CatalogExtraChargeConfiguration());
            builder.ApplyConfiguration(new GalleryItemConfiguration());
            builder.ApplyConfiguration(new FeedbackConfiguration());
            builder.ApplyConfiguration(new FeedbackFilesConfiguration());
            builder.ApplyConfiguration(new SettingsConfiguration());
            builder.ApplyConfiguration(new VideoGalleryConfiguration());
            builder.ApplyConfiguration(new VideoConfiguration());
            builder.ApplyConfiguration(new VideoGalleryVideosConfiguration());
            builder.ApplyConfiguration(new UserDataConfiguration());
            builder.ApplyConfiguration(new FaqConfiguration());
            builder.ApplyConfiguration(new FaqSectionConfiguration());
            builder.ApplyConfiguration(new FaqSectionContentConfiguration());
            builder.ApplyConfiguration(new UrlAccessConfiguration());
            builder.ApplyConfiguration(new BannerItemConfiguration());
            builder.ApplyConfiguration(new NewUsersCounterConfiguration());
            builder.ApplyConfiguration(new AppliedImportConfiguration());
            builder.ApplyConfiguration(new PromoLinkConfiguration());
            builder.ApplyConfiguration(new CatalogConfiguration());
            builder.ApplyConfiguration(new CatalogItemConfiguration());
            builder.ApplyConfiguration(new ModelCatalogOrderConfiguration());
            builder.ApplyConfiguration(new CatalogItemDescriptorConfiguration());
            builder.ApplyConfiguration(new CartItemConfiguration());
            builder.ApplyConfiguration(new ModelCatalogsInvisibilityConfiguration());
            builder.ApplyConfiguration(new BarcodeSqlViewConfiguration());
            builder.ApplyConfiguration(new ModelConfiguration());
            builder.ApplyConfiguration(new ModelCategoryConfiguration());
            builder.ApplyConfiguration(new MaterialConfiguration());
        }

        private static void Apply1CEntitiesConfiguration(ModelBuilder builder) {
            builder.ApplyConfiguration(new ColorsConfiguration());
            builder.ApplyConfiguration(new ColorGroupConfiguration());
            builder.ApplyConfiguration(new CatalogItemBriefConfiguration());
            builder.ApplyConfiguration(new CatalogItemFullConfiguration());
            builder.ApplyConfiguration(new ModelToCatalogMapConfiguration());
            builder.ApplyConfiguration(new ModelToCategotyMapConfiguration());
            builder.ApplyConfiguration(new Category1CConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new UserSqlViewConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ModelsInCatalogCounterConfiguration());
            builder.ApplyConfiguration(new ModelParamsConfiguration());
            builder.ApplyConfiguration(new CartItemSqlViewConfiguration());
            builder.ApplyConfiguration(new ClientRequestItemSqlViewConfiguration());
            builder.ApplyConfiguration(new ClientManufacturingSqlViewConfiguration());
            builder.ApplyConfiguration(new ClientInProductionItemSqlViewConfiguration());
            builder.ApplyConfiguration(new ClientReservationItemSqlViewConfiguration());
            builder.ApplyConfiguration(new ClientSalesItemSqlViewConfiguration());
            builder.ApplyConfiguration(new BarcodeShipmentsSqlViewConfiguration());
            builder.ApplyConfiguration(new ShipmentConfiguration());
            builder.ApplyConfiguration(new BarcodeConfiguration());
            builder.ApplyConfiguration(new StorehouseConfiguration());
            builder.ApplyConfiguration(new StorehouseRestsConfiguration());
            builder.ApplyConfiguration(new OrderDetailsFullConfiguration());
            builder.ApplyConfiguration(new CatalogItem1CAmountConfiguration());
            builder.ApplyConfiguration(new CatalogItemForAddConfiguration());
            builder.ApplyConfiguration(new RequestAdminConfiguration());
            builder.ApplyConfiguration(new ManufactureAdminConfiguration());
            builder.ApplyConfiguration(new ReservationAdminConfiguration());
            builder.ApplyConfiguration(new ShipingAdminConfiguration());
            builder.ApplyConfiguration(new InvoiceSqlConfiguration());
            builder.ApplyConfiguration(new InvoiceLineSqlConfiguration());
            builder.ApplyConfiguration(new OrderHistorySqlViewConfiguration());

            builder.ApplyConfiguration(new NewUserConfiguration());
            builder.ApplyConfiguration(new NewUserSqlViewConfiguration());
            builder.ApplyConfiguration(new UserNewPasswordConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderDetailsConfiguration());
            builder.ApplyConfiguration(new OrderAdminConfiguration());
            builder.ApplyConfiguration(new OrderStatsConfiguration());
            builder.ApplyConfiguration(new ClientOrdersTotalsSqlViewConfiguration());

            builder.ApplyConfiguration(new ModelPhoto1CConfiguration());
            builder.ApplyConfiguration(new ModelPhotoSqlViewConfiguration());
            builder.ApplyConfiguration(new Favorite1CItemConfiguration());
            builder.ApplyConfiguration(new MoneyDocumentSqlViewConfiguration());
            builder.ApplyConfiguration(new MoneyDocumentItemSqlViewConfiguration());
            builder.ApplyConfiguration(new MoneyDocumentsTotalSqlViewConfiguration());
            builder.ApplyConfiguration(new CollectionConfiguration());
            builder.ApplyConfiguration(new CatalogItemVisibilityConfiguration());
        }
    }
}