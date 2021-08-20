using System;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    public class OrderHistory {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
    }
}
