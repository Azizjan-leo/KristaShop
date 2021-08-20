using System;
using KristaShop.Common.Enums;

namespace Module.Client.Business.Models {
    public class AuthorizationLinkDTO {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? LoginDate { get; set; }
        public AuthorizationLinkType Type { get; set; }
    }
}
