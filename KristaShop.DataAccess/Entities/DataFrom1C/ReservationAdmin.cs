using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {

    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.ReservationAdminConfiguration"/>
    /// </summary>
    /// 
    public class ReservationAdmin {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime FirstItemDate { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFullName { get; set; }
        public int TotAmount { get; set; }
        public double TotPrice { get; set; }
        public double TotPriceInRub { get; set; }
    }
}
