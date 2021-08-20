using System;
using System.Collections.Generic;

namespace Module.Order.Business.Models {
    public class OrderDTO {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public bool HasExtraPack { get; set; }
        public bool IsProcessedPreorder { get; set; }
        public bool IsProcessedRetail { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceInRub { get; set; }
        public double TotalAmount { get; set; }

        public List<OrderDetailsDTO> Details { get; set; }

        private bool _isEquals(OrderDetailsDTO val1, OrderDetailsDTO val2) {
            if (val1.CatalogId == val2.CatalogId &&
                val1.ModelId == val2.ModelId &&
                val1.ColorId == val2.ColorId &&
                val1.NomenclatureId == val2.NomenclatureId &&
                val1.Price.ToString("N4") == val2.Price.ToString("N4") &&
                val1.PriceInRub.ToString("N4") == val2.PriceInRub.ToString("N4")) {
                return true;
            } else {
                return false;
            }
        }

        public void GroupOrderDetails() {
            var groupedList = new List<OrderDetailsDTO>();

            foreach (var oItem in Details) {
                int foundIndex = -1;
                for (int i = 0; i < groupedList.Count; i++) {
                    if (_isEquals(oItem, groupedList[i])) {
                        foundIndex = i;
                        break;
                    }
                }
                if (foundIndex >= 0) {
                    groupedList[foundIndex].Amount += oItem.Amount;
                    groupedList[foundIndex].TotalPrice += oItem.TotalPrice;
                    groupedList[foundIndex].TotalPriceInRub += oItem.TotalPriceInRub;
                } else {
                    groupedList.Add(new OrderDetailsDTO(oItem));
                }
            }

            Details = groupedList;
        }
    }
}
