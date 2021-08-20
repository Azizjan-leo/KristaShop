using System;
using KristaShop.Common.Enums;

namespace KristaShop.Common.Models.DTOs {
    public class ManagerAccessDTO {
        public Guid Id { get; set; }
        public int ManagerId { get; set; }
        public int AccessToManagerId { get; set; }
        public ManagerAccessToType AccessTo { get; set; }
    }
}