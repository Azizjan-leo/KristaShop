using System;
using KristaShop.Common.Interfaces.Models;

namespace KristaShop.DataAccess.Entities.Interfaces.DataFrom1C {
    public interface IShipmentsSqlView : ICountableCatalogItem {
        int Id { get; set; }
        int UserId { get; set; }
        string PhotoByColor { get; set; }
        string MainPhoto { get; set; }
        string ColorName { get; set; }
        string ColorPhoto { get; set; }
        string ColorValue { get; set; }
        int PartsCount { get; set; }
        DateTime SaleDate { get; set; }
    }
}