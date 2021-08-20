namespace KristaShop.Common.Interfaces.Settings {
    public interface IPartnersSettings {
        string PartnershipRequestAcceptedToProcess { get; set; }
        string PartnershipRequestRejected { get; set; }
        string PartnershipRequstActiveRequest { get; set; }
        double DefaultPartnerPaymentRate { get; set; }
    }
}