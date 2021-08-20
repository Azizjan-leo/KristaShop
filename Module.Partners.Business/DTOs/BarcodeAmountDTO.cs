using System;
using System.Collections.Generic;
using KristaShop.Common.Models.DTOs;

namespace Module.Partners.Business.DTOs {
    public class BarcodeAmountDTO {
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public List<KristaShop.Common.Models.DTOs.BarcodeAmountDTO> Items { get; set; }
    }
}