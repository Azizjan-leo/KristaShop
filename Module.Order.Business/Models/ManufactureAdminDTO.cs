namespace Module.Order.Business.Models {
    public class ManufactureAdminDTO {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFullName { get; set; }
        public int TotAmount { get; set; }
        public double TotPrice { get; set; }
        public double TotPriceInRub { get; set; }

    }
}
