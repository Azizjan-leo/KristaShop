namespace KristaShop.Common.Models.Session {
    public interface IBaseSession {
        public int UserId { get; set; }
        public string Login { get; set; }
        public bool IsRoot { get; set; }
        public bool IsManager { get; set; }
        public bool IsGuest { get; }
        public LinkSignIn Link { get; set; }
    }
}