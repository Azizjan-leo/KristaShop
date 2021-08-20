using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.MenuItemConfiguration"/>
    /// </summary>
    public class MenuItem : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public MenuType MenuType { get; set; }
        public string Title { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public string BadgeTarget { get; set; }
        public Guid? ParentId { get; set; }

        public MenuItem ParentItem { get; set; }
        public ICollection<MenuItem> ChildItems { get; set; }

        public string Url => $"{(string.IsNullOrEmpty(AreaName) ? string.Empty : $"/{AreaName}")}/{ControllerName}/{ActionName}";

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}