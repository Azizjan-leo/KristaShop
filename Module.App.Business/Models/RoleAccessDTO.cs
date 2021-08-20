using System;

namespace Module.App.Business.Models {
    public class RoleAccessDTO {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAccessGranted { get; set; }
        public string Description { get; set; }
    }
}
