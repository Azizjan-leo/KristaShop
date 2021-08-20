namespace Module.Order.Business.Models {
    public class UserCartTotalsDTO {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int TotalItemsCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceRub { get; set; }
    }
}