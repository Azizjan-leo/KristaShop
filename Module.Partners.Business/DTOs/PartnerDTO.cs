using System;
using KristaShop.Common.Extensions;

namespace Module.Partners.Business.DTOs {
    public class PartnerDTO {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CityName { get; set; }
        public string MallAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string ManagerName { get; set; }
        public int UserId { get; set; }
        public int ShipmentsItemsCount { get; set; }
        public double ShipmentsItemsSum { get; set; }
        public int StorehouseItemsCount { get; set; }
        public int DebtItemsCount { get; set; }
        public double DebtItemsSum { get; set; }
        public DateTimeOffset? RevisionDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public string RevisionDateFormatted => RevisionDate.ToBasicString();
        public string PaymentDateFormatted => PaymentDate.ToBasicString();
    }
}