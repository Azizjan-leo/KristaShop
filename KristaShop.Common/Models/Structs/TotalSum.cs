using System;
using System.Globalization;

namespace KristaShop.Common.Models.Structs {
    public readonly struct TotalSum {
        public double TotalPrice { get; }
        public double TotalPriceInRub { get; }
        public double PrepayPercent { get; }
        
        public TotalSum(double totalPrice, double totalPriceInRub) {
            TotalPrice = totalPrice;
            TotalPriceInRub = totalPriceInRub;
            PrepayPercent = GlobalConstant.GeneralPrepayPercent;
        }
        
        public TotalSum(double totalPrice, double totalPriceInRub, int prepayPercent) {
            TotalPrice = totalPrice;
            TotalPriceInRub = totalPriceInRub;
            PrepayPercent = (double) prepayPercent / 100;
        }

        private TotalSum(double totalPrice, double totalPriceInRub, double prepayPercent) {
            TotalPrice = totalPrice;
            TotalPriceInRub = totalPriceInRub;
            PrepayPercent = prepayPercent;
        }

        public double GetTotalPricePrepay() {
            return TotalPrice * PrepayPercent;
        }

        public double GetTotalPriceInRubPrepay() {
            return TotalPriceInRub * PrepayPercent;
        }

        public string PrepayPercentAsString() {
            return Math.Round(PrepayPercent * 100).ToString(CultureInfo.InvariantCulture);
        }

        public static TotalSum operator +(TotalSum a, TotalSum b) {
            return new(a.TotalPrice + b.TotalPrice, a.TotalPriceInRub + b.TotalPriceInRub, (a.PrepayPercent + b.PrepayPercent) / 2);
        }

        public static TotalSum operator -(TotalSum a, TotalSum b) {
            return new(a.TotalPrice - b.TotalPrice, a.TotalPriceInRub - b.TotalPriceInRub, (a.PrepayPercent + b.PrepayPercent) / 2);
        }
    }
}
