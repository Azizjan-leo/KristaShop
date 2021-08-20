using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.VideoGalleryVideosConfiguration"/>
    /// </summary>
    public class VideoGalleryVideos : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public Guid GalleryId { get; set; }
        public Guid VideoId { get; set; }
        public int Order { get; set; }

        public VideoGallery Gallery { get; set; }
        public Video Video { get; set; }

        public VideoGalleryVideos() { }

        public VideoGalleryVideos(bool generateId) {
            if (generateId) GenerateKey();
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}
