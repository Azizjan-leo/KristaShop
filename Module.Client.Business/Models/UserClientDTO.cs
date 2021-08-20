using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace Module.Client.Business.Models {
    public class UserClientDTO {
        public int UserId { get; set; }
        public Guid NewUserId { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string NewCity { get; set; }
        public string Phone { get; set; }
        public string MallAddress { get; set; }
        public string CompanyAddress { get; set; }
        public string Email { get; set; }
        public double Discount { get; set; }
        public bool CartStatus { get; set; }
        public DateTimeOffset LastSignIn { get; set; }
        public DateTime? CreateDate { get; set; }
        public double Balance { get; set; }
        public double BalanceInRub { get; set; }
        public Dictionary<CatalogType, bool> Catalogs { get; set; }
        public bool IsActive => UserId > 0;
    }
}
