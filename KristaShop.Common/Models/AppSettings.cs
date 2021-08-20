using KristaShop.Common.Interfaces;
using KristaShop.Common.Models.Structs;

namespace KristaShop.Common.Models {
    public class AppSettings : IAppSettings {
        // Dynamic Pages Url settings
        public string TermsOfUse { get; set; }
        public string DeliveryDetails { get; set; }
        public string PaymentDetails { get; set; }
        public string FooterContacts { get; set; }
        public string CategoriesDescription { get; set; }
        public string OpenCatalogSearchDescription { get; set; }

        // Social Url settings
        public string KristaInstagram { get; set; }
        public string KristaVk { get; set; }
        public string KristaFacebook { get; set; }
        public string KristaYoutube { get; set; }
        public string KristaYoutubeSubscribe { get; set; }

        // Messages
        public string CartSuccess { get; set; }

        // PreorderParts
        public CatalogItemsPartition PreorderParts { get; set; }
        public int FrontCatalogOnPageCount { get; set; }
        public bool IsRegistrationActive { get; set; }
        public string InactiveRegistrationMessage { get; set; }
        
        //Partners
        public string PartnershipRequestAcceptedToProcess { get; set; }
        public string PartnershipRequestRejected { get; set; }
        public string PartnershipRequstActiveRequest { get; set; }
        public double DefaultPartnerPaymentRate { get; set; }
    }
}
