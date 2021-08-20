namespace KristaShop.Common.Models.DTOs {
    public class PartnerSalesReportItem {
        public int PartnerId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string MallAddress { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double Sum { get; set; }
    }
}