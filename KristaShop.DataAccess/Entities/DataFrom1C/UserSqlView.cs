using System;
using KristaShop.Common.Helpers;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="UserSqlViewConfiguration"/>
    /// </summary>
    public class UserSqlView {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Date { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string MallAddress { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public bool IsManager { get; set; }
        public bool IsPartner { get; set; }
        public DateTimeOffset LastSignIn { get; set; }
        public string CreateDate { get; set; }
        public string[] CatalogsAccess { get; set; }
        public double Balance { get; set; }
        public double BalanceInRub { get; set; }

        public bool IsPasswordValid(string password) {
            var res = Password.Equals(HashHelper.TransformPassword(password));
            return res;
        }
    }
}
