using System;
using System.Collections.Generic;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="PartnerConfiguration"/>
    /// </summary>
    public class Partner : EntityBase<int> {
        public int UserId { get; set; }
        public DateTimeOffset DateApproved { get; set; }
        public double PaymentRate { get; set; }
        public User User { get; set; }
        public ICollection<Document> Documents { get; set; }

        public Partner() { }

        public Partner(int userId, double paymentRate) {
            UserId = userId;
            DateApproved = DateTimeOffset.Now;
            PaymentRate = paymentRate;
        }

        public override int GetId() {
            return UserId;
        }
    }
}