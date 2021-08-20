using System;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.ManagerDetailsSqlViewConfiguration"/>
    /// </summary>
    public class ManagerDetailsSqlView {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegistrationsQueueNumber { get; set; }
        public string NotificationsEmail { get; set; }
        public bool SendNewRegistrationsNotification { get; set; }
        public bool SendNewOrderNotification { get; set; }
        public Guid RoleId { get; set; }
    }
}
