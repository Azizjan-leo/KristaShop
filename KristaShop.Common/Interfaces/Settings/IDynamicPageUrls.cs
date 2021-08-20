namespace KristaShop.Common.Interfaces.Settings {
    public interface IDynamicPageUrls {
        string TermsOfUse { get; set; }
        string DeliveryDetails { get; set; }
        string PaymentDetails { get; set; }
        string FooterContacts { get; set; }
        string CategoriesDescription { get; set; }
        string OpenCatalogSearchDescription { get; set; }
    }
}
