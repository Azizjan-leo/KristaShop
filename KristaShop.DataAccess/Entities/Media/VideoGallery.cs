using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.VideoGalleryConfiguration"/>
    /// </summary>
    public class VideoGallery : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PreviewPath { get; set; }
        public string VideoPath { get; set; }
        public bool IsVisible { get; set; }
        public bool IsOpen { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }

        public ICollection<VideoGalleryVideos> VideoGalleryVideos { get; set; }

        public VideoGallery() { }

        public VideoGallery(bool generateId) {
            if (generateId) GenerateKey();
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}
