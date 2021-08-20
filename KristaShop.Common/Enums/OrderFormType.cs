using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Models;

namespace KristaShop.Common.Enums {
    public enum OrderFormType {
        [Display(Name = "Наличие")] 
        InStock = 1,

        [Display(Name = "Предзаказ")] 
        Preorder = 2
    }

    public static class OrderFormTypeExtension {
        public static string ToReadableString(this OrderFormType type) {
            switch (type) {
                case OrderFormType.InStock:
                    return "Наличие";
                case OrderFormType.Preorder:
                    return "Предзаказ";
                default:
                    return "---";
            }
        }

        public static List<LookUpItem<OrderFormType, string>> GetOrderFormLookup() {
            return new List<LookUpItem<OrderFormType, string>> {
                new LookUpItem<OrderFormType, string>(OrderFormType.InStock, OrderFormType.InStock.ToReadableString()),
                new LookUpItem<OrderFormType, string>(OrderFormType.Preorder, OrderFormType.Preorder.ToReadableString())
            };
        }
    }
}