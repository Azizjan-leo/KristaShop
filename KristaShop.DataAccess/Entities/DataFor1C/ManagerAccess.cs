using KristaShop.Common.Enums;
using System;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.ManagerAccessConfiguration"/>
    /// </summary>
    public class ManagerAccess {
        public Guid Id { get; set; }
        public int ManagerId { get; set; }
        public int AccessToManagerId { get; set; }
        public ManagerAccessToType AccessTo { get; set; }

        public ManagerDetails ManagerDetails { get; set; }
    }
}
