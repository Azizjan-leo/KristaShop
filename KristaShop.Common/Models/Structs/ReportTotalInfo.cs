using System;
using System.Globalization;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Models.Structs {
    public readonly struct ReportTotalInfo {
        public int TotalAmount { get; }
        public double TotalSum { get; }
        public double TotalSumInRub { get; }
        public double PrepayPercent { get; }

        public ReportTotalInfo(int totalAmount, double totalSum, double totalSumInRub) {
            TotalAmount = totalAmount;
            TotalSum = totalSum;
            TotalSumInRub = totalSumInRub;
            PrepayPercent = GlobalConstant.GeneralPrepayPercent;
        }

        public ReportTotalInfo(int totalAmount, double totalSum, double totalSumInRub, double prepayPercent) {
            TotalAmount = totalAmount;
            TotalSum = totalSum;
            TotalSumInRub = totalSumInRub;
            PrepayPercent = prepayPercent;
        }

        public static ReportTotalInfo Default => new(0, 0, 0);
        
        public double GetTotalPricePrepay() {
            return TotalSum * PrepayPercent;
        }

        public double GetTotalPriceInRubPrepay() {
            return TotalSumInRub * PrepayPercent;
        }

        public string PrepayPercentAsString() {
            return Math.Round(PrepayPercent * 100).ToString(CultureInfo.InvariantCulture);
        }
        
        public static ReportTotalInfo operator +(ReportTotalInfo a, ReportTotalInfo b) {
            return new(a.TotalAmount + b.TotalAmount,
                a.TotalSum + b.TotalSum,
                a.TotalSumInRub + b.TotalSumInRub,
                (a.PrepayPercent + b.PrepayPercent) / 2);
        }

        public override string ToString() {
            return $"{TotalAmount} единиц на сумму {TotalSum.ToTwoDecimalPlaces()}$";
        }
    }
}