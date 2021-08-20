using System;
using System.Collections.Generic;

namespace Module.Order.Business.Models {
    public class OrderAdminDTO {
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
        public double TotalSum { get; set; }
        public double TotalSumInRub { get; set; }
        public double PreorderTotalSum { get; set; }
        public double PreorderTotalSumInRub { get; set; }
        public double RetailTotalSum { get; set; }
        public double RetailTotalSumInRub { get; set; }
        public int PreorderAmount { get; set; }
        public int RetailAmount { get; set; }
        public bool HasExtraPack { get; set; }
        public string UserComments { get; set; }
        public bool IsReviewed { get; set; }
        public int TotalAmount => PreorderAmount + RetailAmount;
        public bool IsUnprocessed { get; set; }
        public double PrepayTotalSum { get; set; }

        public List<OrderDetailsDTO> Details { get; set; }
    }
}
