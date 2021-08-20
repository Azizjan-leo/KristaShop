using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Views;
using Module.Common.Business.Models;
using Module.Common.Business.Models.Adapters;

namespace Module.Order.Business.Models.Adapters {
    public class OrderTotalsAdapter : IEntityToDtoAdapter<List<ReportTotals>, ReportTotalsDTO> {
        public ReportTotalsDTO Convert(List<ReportTotals> source) {
            var result = new ReportTotalsDTO();

            var amount = 0;
            var price = 0d;
            var priceInRub = 0d;
            foreach (var item in source) {
                var catalogTotals = new ReportTotalInfo(item.TotalAmount, item.TotalPrice, item.TotalPriceInRub);
                amount += item.TotalAmount;
                price += item.TotalPrice;
                priceInRub += item.TotalPriceInRub;
                switch (item.CatalogId) {
                    case CatalogType.Preorder:
                        result.PreorderTotals = catalogTotals;
                        break;
                    case CatalogType.InStockLines:
                        result.InStockLinesTotals = catalogTotals;
                        break;
                    case CatalogType.InStockParts:
                        result.InStockPartsTotals = catalogTotals;
                        break;
                }
            }

            result.Totals = new ReportTotalInfo(amount, price, priceInRub);
            return result;
        }
    }
}
