using KristaShop.Common.Enums;

namespace KristaShop.Common.Models.Session {
    public class LinkSignIn {
        public bool IsSignByLink { get; set; }
        public AuthorizationLinkType Type { get; set; }
        public string Code { get; set; }

        public LinkSignIn() { }

        public LinkSignIn(AuthorizationLinkType type, string code) {
            Type = type;
            Code = code;
            IsSignByLink = type != AuthorizationLinkType.None;
        }

        public bool IsSignedInForChangePassword => IsSignByLink && Type == AuthorizationLinkType.ChangePassword;
    }
}