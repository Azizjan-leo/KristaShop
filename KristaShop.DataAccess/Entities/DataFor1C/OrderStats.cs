using System;
using System.Collections.Generic;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.OrderStatsConfiguration"/>
    /// </summary>
    /// 
    public class OrderStats {
        public int OrdersCount { get; set; }
        public int TotAmountPreorder { get; set; }
        public double TotSumPreorder { get; set; }
        public int TotAmountRetail { get; set; }
        public double TotSumRetail { get; set; }
    }
}
