using KristaShop.Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KristaShop.Common.Models.Requests {
    public class IncomeShipmentItemsRequest {
        [Range(typeof(DateTime), "01/01/2021", "01/01/2100", ErrorMessage="Дата указана не верно")]
        public DateTime ReservationDate { get; set; }
        [Required]
        public List<BarcodeAmountDTO> StorehouseIncomes { get; set; }
    }
}