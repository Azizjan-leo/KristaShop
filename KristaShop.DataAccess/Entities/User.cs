using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    ///     Configuration file for this entity <see cref="Configurations.UserConfiguration" />
    /// </summary>
    public class User : EntityBase<int>, IIdentityKeyGeneratable {
        private bool _accessToInStockLinesCatalog;
        private bool _accessToInStockPartsCatalog; 
        private bool _accessToPreorder;
        private bool _accessToRfInStockLinesCatalog;
        private bool _accessToRfInStockPartsCatalog;
        private bool _accessToSaleLinesCatalog;
        private bool _accessToSalePartsCatalog;
        private string _decryptedPassword;

        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MallAddress { get; set; }
        public int? CityId { get; set; }
        public double Balance { get; set; }
        public double BalanceInRub { get; set; }
        public bool IsManager { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreateDate { get; set; }

        public UserData Data { get; set; }
        public Manager Manager { get; set; }
        public City City { get; set; }

        public bool IsPasswordValid(string password) {
            var res = Password.Equals(HashHelper.TransformPassword(password));
            return res;
        }

        public Dictionary<CatalogType, bool> GetAccessesToCatalogs() {
            return new() {
                {CatalogType.InStockLines, _accessToInStockLinesCatalog},
                {CatalogType.InStockParts, _accessToInStockPartsCatalog},
                {CatalogType.Preorder, _accessToPreorder},
                {CatalogType.RfInStockLines, _accessToRfInStockLinesCatalog },
                {CatalogType.RfInStockParts, _accessToRfInStockPartsCatalog },
                {CatalogType.SaleLines, _accessToSaleLinesCatalog },
                {CatalogType.SaleParts, _accessToSalePartsCatalog },
            };
        }

        public void SetCatalogsAccesses(Dictionary<CatalogType, bool> catalogs) {
            foreach (var catalog in catalogs) _setCatalogAccess(catalog.Key, catalog.Value);
        }

        public void SetCatalogAccess(CatalogType catalogId, bool hasAccess) {
            _setCatalogAccess(catalogId, hasAccess);
        }

        public void SetPassword(string decryptedPassword) {
            _decryptedPassword = decryptedPassword;
            Password = HashHelper.TransformPassword(decryptedPassword);
        }

        public List<CatalogType> GetOnlyAvailableCatalogs() {
            var result = GetAccessesToCatalogs().Where(x => x.Value).Select(x => x.Key).ToList();

            if (!result.Any()) result.Add(CatalogType.Open);

            return result;
        }

        public bool HasAccessTo(CatalogType catalogId) {
            var accesses = GetAccessesToCatalogs();
            return accesses.ContainsKey(catalogId) && accesses[catalogId];
        }

        private void _setCatalogAccess(CatalogType catalogId, bool hasAccess) {
            switch (catalogId) {
                case CatalogType.InStockLines:
                    _accessToInStockLinesCatalog = hasAccess;
                    break;
                case CatalogType.InStockParts:
                    _accessToInStockPartsCatalog = hasAccess;
                    break;
                case CatalogType.Preorder:
                    _accessToPreorder = hasAccess;
                    break;
                case CatalogType.RfInStockLines:
                    _accessToRfInStockLinesCatalog = hasAccess;
                    break;  
                case CatalogType.RfInStockParts:
                    _accessToRfInStockPartsCatalog = hasAccess;
                    break;
                case CatalogType.SaleLines:
                    _accessToSaleLinesCatalog = hasAccess;
                    break;
                case CatalogType.SaleParts:
                    _accessToSalePartsCatalog = hasAccess;
                    break;
            }
        }

        public void SetPrimaryKey(int value) {
            Id = value;
        }

        public override int GetId() {
            return Id;
        }
    }
}