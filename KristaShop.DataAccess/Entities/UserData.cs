using System;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.UserDataConfiguration"/>
    /// </summary>
    public class UserData {
        public int UserId { get; set; }
        public DateTimeOffset LastSignIn { get; set; }
        public User User { get; set; }

        public UserData() { }

        public UserData(int userId) {
            UserId = userId;
            LastSignIn = DateTimeOffset.Now;
        }
    }
}
