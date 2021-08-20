using System;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.OrderAdminConfiguration"/>
    /// </summary>
    public class OrderAdmin {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public bool UserHasCartItems { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFullName { get; set; }
        public bool IsProcessedPreorder { get; set; }
        public bool IsProcessedRetail { get; set; }
        public double PreorderTotalSum { get; set; }
        public double PreorderTotalSumInRub { get; set; }
        public double RetailTotalSum { get; set; }
        public double RetailTotalSumInRub { get; set; }
        public int PreorderAmount { get; set; }
        public int RetailAmount { get; set; }
        public bool HasExtraPack { get; set; }
        public string UserComments { get; set; }
        public bool IsReviewed { get; set; }
        public double TotalSum => PreorderTotalSum + RetailTotalSum;
        public double TotalSumInRub => PreorderTotalSumInRub + RetailTotalSumInRub;
        public bool IsUnprocessed => (PreorderAmount > 0 && !IsProcessedPreorder) || (RetailAmount > 0 && !IsProcessedRetail);
    }
}