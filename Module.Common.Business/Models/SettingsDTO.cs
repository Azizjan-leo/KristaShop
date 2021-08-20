using System;

namespace Module.Common.Business.Models {
    public class SettingsDTO {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}