using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.StorehouseConfiguration"/>
    /// </summary>
    public class Storehouse {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCollective { get; set; }
        public int Priority { get; set; }
    }
}
