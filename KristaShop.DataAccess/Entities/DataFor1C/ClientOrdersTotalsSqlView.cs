namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.ClientOrdersTotalsSqlViewConfiguration"/>
    /// </summary>
    public class ClientOrdersTotalsSqlView {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public bool UserHasCartItems { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int PreorderTotalAmount { get; set; }
        public int InStockTotalAmount { get; set; }
        public double PreorderTotalSum { get; set; }
        public double PreorderTotalSumInRub { get; set; }
        public double InStockTotalSum { get; set; }
        public double InStockTotalSumInRub { get; set; }
        public int TotalAmount => PreorderTotalAmount + InStockTotalAmount;
        public double TotalSum => PreorderTotalSum + InStockTotalSum;
        public double TotalSumInRub => PreorderTotalSumInRub + InStockTotalSumInRub;
    }
}