using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.VideoConfiguration"/>
    /// </summary>
    public class Video : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string PreviewPath { get; set; }
        public string VideoPath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }

        public ICollection<VideoGalleryVideos> VideoGalleryVideos { get; set; }

        public Video() { }

        public Video(bool generateId) {
            if(generateId) GenerateKey();
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}
