namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.UserNewPasswordConfiguration"/>
    /// </summary>
    public class UserNewPassword {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string PasswordSrc { get; set; }

        public UserNewPassword() { }

        public UserNewPassword(int userId, string password, string passwordSrc) {
            UserId = userId;
            Password = password;
            PasswordSrc = passwordSrc;
        }
    }
}
