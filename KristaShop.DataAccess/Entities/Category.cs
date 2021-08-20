using System;
using KristaShop.DataAccess.Configurations;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="CategoryConfiguration"/>
    /// </summary>
    public class Category {
        public Guid Id { get; set; }
        public int CategoryId1C { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; }
    }
}