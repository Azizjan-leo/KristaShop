using System;

namespace KristaShop.Common.Models.Session {
    public class GuestSession : UserSession, IGuestSession {
        public Guid Id { get; set; }
        public override bool IsGuest => true;
        public GuestAccessInfo GuestAccessIngo { get; set; }

        public override int UserId {
            get {
                var tmp = Id.GetHashCode();
                if (tmp == 0) tmp = -63542658;
                return tmp > 0 ? -tmp : tmp;
            }
            set {

            }
        }
    }
}