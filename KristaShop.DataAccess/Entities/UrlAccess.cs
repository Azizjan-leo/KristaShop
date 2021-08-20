using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.UrlAccessConfiguration"/>
    /// </summary>
    public class UrlAccess : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool ForManager { get; set; }
        public bool ForRootOnly { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}