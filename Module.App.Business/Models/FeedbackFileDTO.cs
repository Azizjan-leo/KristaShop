using System;

namespace Module.App.Business.Models {
    public class FeedbackFileDTO {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Filename { get; set; }
        public string VirtualPath { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
