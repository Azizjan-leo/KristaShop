using System;
using KristaShop.Common.Enums;

namespace Module.App.Business.Models {
    public class FeedbackDTO {
        public Guid Id { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public bool Viewed { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public string FormattedDate { get; set; }
        public FeedbackType Type { get; set; }
        public string FeedbackType { get; set; }
        public long FilesCount { get; set; }
    }
}