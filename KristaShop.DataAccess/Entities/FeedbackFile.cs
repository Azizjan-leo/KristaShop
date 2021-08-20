using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.FeedbackFilesConfiguration"/>
    /// </summary>
    public class FeedbackFile : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Filename { get; set; }
        public string VirtualPath { get; set; }
        public DateTime CreateDate { get; set; }

        public Feedback Feedback { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}