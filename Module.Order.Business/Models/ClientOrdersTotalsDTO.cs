namespace Module.Order.Business.Models {
    public class ClientOrdersTotalsDTO {
        public string UserId { get; set; }
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
