using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.SettingsConfiguration"/>
    /// </summary>
    public class Settings : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool OnlyRootAccess { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}