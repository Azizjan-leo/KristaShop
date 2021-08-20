using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class ItemsGroupedBase<TItems, TTotals> where TItems : class {
        public List<TItems> Items { get; set; }
        public TTotals Totals { get; set; }

        public ItemsGroupedBase() {
            Items = new List<TItems>();
        }
    }

    public class ItemsGrouped<TItems> : ItemsGroupedBase<TItems, ReportTotalInfo> where TItems : class { }

    public class UnprocessedItemsGrouped<TItems> : ItemsGroupedBase<TItems, ReportTotalInfo> where TItems : class {
        public ReportTotalInfo PreorderTotals { get; set; }
        public ReportTotalInfo InStockTotals { get; set; }
    }

    public class ItemsGroupedWithDate<T> : ItemsGrouped<T> where T : class {
        public DateTime CreateDate { get; set; }
    }

    public class ItemsGroupedWithName<T> : ItemsGrouped<T> where T : class {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemsGroupedWithDateName<T> : ItemsGrouped<T> where T : class {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ManufactureItemsGrouped<TItems> : ItemsGroupedWithName<TItems> where TItems : class {
        public Dictionary<ManufactureState, ReportTotalInfo> TotalByState { get; set; }

        public ReportTotalInfo GetTotalsByState(params ManufactureState[] states) {
            var result = new ReportTotalInfo(0, 0, 0);
            foreach (var state in states) {
                if (TotalByState.ContainsKey(state)) {
                    result += TotalByState[state];
                }
            }
            return result;
        }
    }
}
