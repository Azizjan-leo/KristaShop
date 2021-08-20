namespace KristaShop.Common.Models.Session {
    public sealed class UnauthorizedSession : UserSession {
        public UnauthorizedSession() {
            User = null;
            IsManager = false;
            IsRoot = false;
            Link = null;
            Login = "Unauthorized";
            ManagerId = -1;
            UserId = 0;
        }
    }
}