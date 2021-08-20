using KristaShop.Common.Enums;
using KristaShop.Common.Models.DTOs;

namespace KristaShop.Common.Models.Session {
    public class UserSession : IBaseSession {
        public virtual int UserId { get; set; }
        public string Login { get; set; }
        public int ManagerId { get; set; }
        public bool IsRoot { get; set; }
        public bool IsManager { get; set; }
        public virtual bool IsGuest => false;
        public LinkSignIn Link { get; set; }
        public bool IsPartner { get; set; }
        public UserSessionInfo User { get; set; }
        public ManagerDetailsDTO ManagerDetails { get; set; }
        
        public static IBaseSession CreateSession(int userId, string login, bool isManager) {
            return new UserSession {
                UserId = userId,
                Login = login,
                IsManager = isManager,
                IsRoot = false,
                Link = new LinkSignIn(AuthorizationLinkType.None, string.Empty)
            };
        }

        public static IBaseSession CreateLinkSession(int userId, string login, LinkSignIn link) {
            return new UserSession {
                UserId = userId,
                Login = login,
                IsManager = false,
                IsRoot = false,
                Link = link
            };
        }

        public static IBaseSession CreateRootSession() {
            return new UserSession {
                UserId = 0,
                Login = "root",
                IsManager = false,
                IsRoot = true,
                Link = new LinkSignIn(AuthorizationLinkType.None, string.Empty)
            };
        }
    }
}