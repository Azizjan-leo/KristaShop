using KristaShop.Common.Implementation.DataAccess;
using System;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.RoleAccessConfiguration"/>
    /// </summary>
    public class RoleAccess : EntityBase<Guid> {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAccessGranted { get; set; }
        public string Description { get; set; }
        public Role Role { get; set; }

        public override Guid GetId() {
            return Id;
        }
    }
}