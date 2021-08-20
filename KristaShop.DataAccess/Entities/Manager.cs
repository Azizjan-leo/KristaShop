using System.Collections.Generic;
using KristaShop.DataAccess.Entities.DataFor1C;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.ManagerConfiguration"/>
    /// </summary>
    public class Manager {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public ManagerDetails Details { get; set; }
        public ICollection<User> Users { get; set; }
    }
}