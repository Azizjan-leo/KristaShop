using System;

namespace Module.App.Admin.Admin.Models {
    public class RoleAccessViewModel {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAccessGranted { get; set; }
        public string Description { get; set; }
        public string ItemKey => $"{Area}.{Controller}.{Action}";

        public override bool Equals(object obj) {
            return obj is RoleAccessViewModel model &&
                   Id.Equals(model.Id) &&
                   RoleId.Equals(model.RoleId) &&
                   Area.Equals(model.Area) &&
                   Controller.Equals(model.Controller) &&
                   Action.Equals(model.Action);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, RoleId, Area, Controller, Action);
        }
    }
}
