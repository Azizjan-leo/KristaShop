using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.AuthorizationLinkConfiguration"/>
    /// </summary>
    public class AuthorizationLink : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? LoginDate { get; set; }
        public AuthorizationLinkType Type { get; set; }

        public AuthorizationLink() {
            Type = AuthorizationLinkType.MultipleAccess;
        }

        public AuthorizationLink(int userId, AuthorizationLinkType type = AuthorizationLinkType.MultipleAccess, int activeInDays = 14) {
            Type = type;
            RecordTimeStamp = DateTime.Now;
            UserId = userId;
            ValidTo = Type == AuthorizationLinkType.SingleAccess ? (DateTime?) null : DateTime.Now.AddDays(activeInDays);
            Code = HashHelper.CalculateMD5Hash(Guid.NewGuid().ToString());
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }

        public void UpdateLoginDate() {
            LoginDate = DateTime.Now;
        }

        public void UpdateActiveDays(int activeInDays = 14) {
            if (Type == AuthorizationLinkType.MultipleAccess || Type == AuthorizationLinkType.ChangePassword) {
                ValidTo = DateTime.Now.AddDays(activeInDays);
            }
        }

        public bool IsValid() {
            return Type == AuthorizationLinkType.SingleAccess && LoginDate == null || ValidTo > DateTime.Now;
        }
    }
}