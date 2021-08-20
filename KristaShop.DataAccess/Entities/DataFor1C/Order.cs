using System;
using System.Collections.Generic;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.OrderConfiguration"/>
    /// </summary>
    public class Order {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public bool HasExtraPack { get; set; }
        public bool IsProcessedPreorder { get; set; }
        public bool IsProcessedRetail { get; set; }
        public string Description { get; set; }
        public bool IsReviewed { get; set; }

        public ICollection<OrderDetails> Details { get; set; }
    }
}
