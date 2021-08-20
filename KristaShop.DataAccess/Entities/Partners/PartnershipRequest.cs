using KristaShop.Common.Interfaces.DataAccess;
using System;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Partners.PartnershipRequestConfiguration"/>
    /// </summary>
    public class PartnershipRequest : IEntityKeyGeneratable {

        public PartnershipRequest(int userId, DateTime requestedDate) {
            UserId = userId;
            RequestedDate = requestedDate;
        }
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime RequestedDate { get; set; }
        public bool IsAcceptedToProcess { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? AnsweredDate { get; set; }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}