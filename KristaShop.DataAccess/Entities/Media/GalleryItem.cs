using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.GalleryItemConfiguration"/>
    /// </summary>
    public class GalleryItem : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}