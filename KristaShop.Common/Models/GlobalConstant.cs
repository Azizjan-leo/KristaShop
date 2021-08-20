namespace KristaShop.Common.Models {
    public static class GlobalConstant {
        public const double Epsilon = 1E-6;
        public const string NotificationKey = "AlertResult";
        public const string MetaKey = "MetaData";
        public const string MenuItemParentId = "MenuItemParentId";
        public const string BreadcrumbKey = "BreadcrumbNode";

        public const string AdminArea = "Admin";
        public const string PartnersArea = "Partners";

        public const int GeneralPrepayPercentValue = 30;
        public const double GeneralPrepayPercent = 0.3d;

        public static class Session
        {
            public const string BackendUserClaimName = "user-key";
            public const string FrontendUserClaimName = "user-key";
            public const string FrontendGuestClaimName = "guest-key";

            public static string GetKeyByScheme(string scheme) {
                if (scheme.Equals(FrontendScheme)) {
                    return FrontendUserClaimName;
                } else if(scheme.Equals(BackendScheme)) {
                    return BackendUserClaimName;
                } else if (scheme.Equals(FrontendGuestScheme)) {
                    return FrontendGuestClaimName;
                }

                return string.Empty;
            }

            public const string FrontendScheme = "FrontendScheme";
            public const string FrontendGuestScheme = "FrontendGuestScheme";
            public const string AllFrontSchemes = "FrontendScheme,FrontendGuestScheme";
            public const string BackendScheme = "BackendScheme";
        }

        public const string SECRET_CODE = "u3og=ckxhb?YSCoMA;tFfP!Q$l4}Of?HfdHG4?<GM75]A2o;^,SHF)";
        public const string CRYPTO_SALT = "7q3t45msdrWSE$%Ywtwy4%&#E%^*UE%^uied56i8ehasedfghbZXDT";
        public static class FeatureFlags {
            public const string FeaturePartners = "FeaturePartners";
            public const string FeaturePartnersPromo = "FeaturePartnersPromo";
            public const string FeatureAdvancedFunctionality = "FeatureAdvancedFunctionality";
        }
    }
}