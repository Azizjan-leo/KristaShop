using System;
using System.Text.RegularExpressions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.DataFor1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFor1C.NewUserConfiguration"/>
    /// </summary>
    public class NewUser : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public int? CityId { get; set; }
        public string NewCity { get; set; }
        public string MallAddress { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? ManagerId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreateDate { get; set; }

        public NewUser() {
            CreateDate = DateTime.Now;
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }

        public string CreateLoginFromName(bool fullPhone = false) {
            string tmpPhone = Phone.Trim();
            tmpPhone = Regex.Replace(tmpPhone, "[^0-9]", "");

            while (tmpPhone.Length < 3) tmpPhone = "0" + tmpPhone;
            string phonePart = (fullPhone ? Phone : tmpPhone.Substring(tmpPhone.Length - 3));

            return ($"{FullName.Split(" ")[0]}{phonePart}").ToLower().ToValidLogin();
        }
    }
}
