namespace KristaShop.Common.Interfaces.Settings {
    public interface IMessages {
        string CartSuccess { get; set; }
        bool IsRegistrationActive { get; set; }
        string InactiveRegistrationMessage { get; set; }
    }
}
