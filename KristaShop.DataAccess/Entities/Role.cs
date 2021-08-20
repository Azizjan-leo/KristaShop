using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFor1C;
using System;
using System.Collections.Generic;
using KristaShop.Common.Implementation.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.RoleConfiguration"/>
    /// </summary>
    public class Role : EntityBase<Guid>, IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleAccess> Accesses { get; set; }
        public ICollection<ManagerDetails> ManagerDetails { get; set; }

        public Role() { }

        public Role(Guid id, string name) {
            Id = id;
            Name = name;
        }
        
        public void GenerateKey() {
            Id = Guid.NewGuid();
        }

        public override Guid GetId() {
            return Id;
        }
    }
}
