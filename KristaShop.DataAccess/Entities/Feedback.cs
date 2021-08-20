using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.FeedbackConfiguration"/>
    /// </summary>
    public class Feedback : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public bool Viewed { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public Guid ReviewerUserId { get; set; }
        public DateTime? ViewTimeStamp { get; set; }
        public FeedbackType Type { get; set; }

        public ICollection<FeedbackFile> Files { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}