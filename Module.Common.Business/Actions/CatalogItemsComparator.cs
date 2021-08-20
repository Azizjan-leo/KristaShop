using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Actions {
    public static class CatalogItemsComparator {
        public static (IList<TSourceItem> excess, IList<TDestinationItem> deficiency)
            CheckForExcessAndDeficiency<TSourceItem, TDestinationItem>
                (IEnumerable<TSourceItem> compareFrom, IEnumerable<TDestinationItem> compareTo) 
                where TSourceItem : class, ICountableCatalogItem
                where TDestinationItem : class, ICountableCatalogItem {
            var uncombinedSourceItems = UnCombineSizeLine(compareFrom);
            var uncombinedDestinationItems = UnCombineSizeLine(compareTo);
            var sourceItems = uncombinedSourceItems.GroupBy(x => x.GetItemKey()).ToDictionary(k => k.Key, v => v.ToList());
            var destinationItems = uncombinedDestinationItems.GroupBy(x => x.GetItemKey()).ToDictionary(k => k.Key, v => v.ToList());

            foreach (var sourceItemGroup in sourceItems) {
                // If destination items list doesn't contains item with such key, then this is excess item
                if (!destinationItems.ContainsKey(sourceItemGroup.Key)) {
                    continue;
                }

                var sourceItemsAmount = sourceItemGroup.Value.Sum(x => x.Amount);
                var destinationItemsAmount = destinationItems[sourceItemGroup.Key].Sum(x => x.Amount);

                foreach (var destinationItem in destinationItems[sourceItemGroup.Key]) {
                    if (sourceItemsAmount <= 0) break;

                    if (destinationItem.Amount - sourceItemsAmount > 0) {
                        destinationItem.Amount -= sourceItemsAmount;
                        sourceItemsAmount = 0;
                    } else {
                        sourceItemsAmount -= destinationItem.Amount;
                        destinationItem.Amount = 0;
                    }
                }

                foreach (var sourceItem in sourceItemGroup.Value) {
                    if (destinationItemsAmount <= 0) break;

                    if (sourceItem.Amount - destinationItemsAmount > 0) {
                        sourceItem.Amount -= destinationItemsAmount;
                        destinationItemsAmount = 0;
                    } else {
                        destinationItemsAmount -= sourceItem.Amount;
                        sourceItem.Amount = 0;
                    }
                }
            }

            var excesses = uncombinedSourceItems.Where(x => x.Amount > 0).ToList();
            var deficiencies = uncombinedDestinationItems.Where(x => x.Amount > 0).ToList();
            return (excesses, deficiencies);
        }
        
        public static IList<TItem> UnCombineSizeLine<TItem>(IEnumerable<TItem> items) where TItem : ICountableCatalogItem {
            var result = new List<TItem>();
            foreach (var item in items) {
                if (!item.Size.IsLine) {
                    result.Add((TItem) item.Clone());
                    continue;
                }

                result.Remove(item);
                foreach (var sizeValue in item.Size.Values) {
                    var clonedItem = (TItem) item.Clone();
                    clonedItem.Size = new SizeValue(sizeValue);
                    clonedItem.Amount = item.Amount;
                    result.Add(clonedItem);
                }
            }

            return result;
        }
    }
}
