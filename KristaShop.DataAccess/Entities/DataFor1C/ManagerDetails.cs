using System;
using System.Collections.Generic;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.ManagerDetailsConfiguration"/>
    /// </summary>
    public class ManagerDetails {
        public int ManagerId { get; set; }
        public int RegistrationsQueueNumber { get; set; }
        public string NotificationsEmail { get; set; }
        public bool SendNewRegistrationsNotification { get; set; }
        public bool SendNewOrderNotification { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Manager Manager { get; set; }

        public ICollection<ManagerAccess> Accesses { get; set; }
    }
}
