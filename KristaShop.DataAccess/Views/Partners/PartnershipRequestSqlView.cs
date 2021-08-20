using System;

namespace KristaShop.DataAccess.Views.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Views.PartnershipRequestSqlViewConfiguration"/>
    /// </summary>
    public class PartnershipRequestSqlView {
        public Guid Id { get; set; } 
        public string FullName { get; set; } 
        public string CityName { get; set; }
        public string MallAddress { get; set; } 
        public string Phone { get; set; } 
        public string Email { get; set; } 
        public string ManagerName { get; set; } 
        public int UserId { get; set; } 
        public DateTime RequestedDate { get; set; }
        public bool IsAcceptedToProcess { get; set; } 
        public bool IsConfirmed { get; set; } 
        public DateTime? AnsweredDate { get; set; } 
        public DateTime LastSignIn { get; set; }
    }
}
