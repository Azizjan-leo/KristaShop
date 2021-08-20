using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;

namespace KristaShop.Common.Models.DTOs {
    public class ManagerDetailsDTO {
        public int Id { get; set; }
        public string Name => Manager?.Name ?? "";
        public int RegistrationsQueueNumber { get; set; }
        public string NotificationsEmail { get; set; }
        public bool SendNewRegistrationsNotification { get; set; }
        public bool SendNewOrderNotification { get; set; }
        public Guid RoleId { get; set; }
        public ManagerDTO Manager { get; set; }
        public IEnumerable<ManagerAccessDTO> Accesses { get; set; }


        public List<int> GetManagerIdsAccessesFor(ManagerAccessToType accessToType) {
            if (Accesses == null) return new List<int> { Id };
            return Accesses.Where(x => x.AccessTo == accessToType).Select(x => x.AccessToManagerId).ToList();
        }
    }
}