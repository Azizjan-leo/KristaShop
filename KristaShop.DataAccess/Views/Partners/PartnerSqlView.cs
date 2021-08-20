using System;

namespace KristaShop.DataAccess.Views.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Views.PartnersSqlViewConfiguration"/>
    /// </summary>
    public class PartnerSqlView {
        public string FullName { get; set; }
        public string CityName { get; set; }
        public string MallAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string ManagerName { get; set; }
        public int UserId { get; set; }
        public int ShipmentsItemsCount { get; set; }
        public double ShipmentsItemsSum { get; set; }
        public int StorehouseItemsCount { get; set; }
        public int DebtItemsCount { get; set; }
        public double DebtItemsSum { get; set; }
        public DateTimeOffset? RevisionDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}
