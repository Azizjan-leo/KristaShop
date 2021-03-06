using System;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.NewUserSqlViewConfiguration"/>
    /// </summary>
    public class NewUserSqlView {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string NewCity { get; set; }
        public string MallAddress { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}