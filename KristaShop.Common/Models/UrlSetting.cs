namespace KristaShop.Common.Models {
    public class UrlSetting {
        public string KristaFileServiceUrl { get; set; }
        public string KristaFileServiceLoadEndpoint { get; set; }
        public string KristaFileServiceDocumentsVirtualPath { get; set; }

        public string KristaWebApiUrl { get; set; }
        public string KristaWebApiRegister { get; set; }
        public string KristaWebApiActivateUser { get; set; }
        public string KristaWebApiBanUser { get; set; }
        public string KristaWebApiChangePassword { get; set; }
        public string KristaWebApiUpdateUser { get; set; }
        public string KristaShopUrl { get; set; }

        public string FileServiceDownloadEndpoint => $"{KristaFileServiceUrl}/{KristaFileServiceLoadEndpoint}";
        public string KristaShopPromoUrl => $"{KristaShopUrl}/Promo/";
    }
}