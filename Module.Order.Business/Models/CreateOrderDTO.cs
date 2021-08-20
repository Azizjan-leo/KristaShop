using System;
using System.Collections.Generic;

namespace Module.Order.Business.Models {
    public class CreateOrderDTO {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public bool HasExtraPack { get; set; }
        public bool IsProcessed { get; set; }
        public string Description { get; set; }

        public List<CartItem1CDTO> CartItems { get; set; }
    }
}
