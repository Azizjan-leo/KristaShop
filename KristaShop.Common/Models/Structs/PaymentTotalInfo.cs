using KristaShop.Common.Extensions;

namespace KristaShop.Common.Models.Structs {
    public readonly struct PaymentTotalInfo {
        public int TotalAmount { get; }
        public double TotalSum { get; }
        public double PaymentRate { get; }

        public PaymentTotalInfo(int totalAmount, double paymentRate) {
            TotalAmount = totalAmount;
            TotalSum = totalAmount * paymentRate;
            PaymentRate = GlobalConstant.GeneralPrepayPercent;
        }

        public PaymentTotalInfo(int totalAmount, double totalSum, double paymentRate) {
            TotalAmount = totalAmount;
            TotalSum = totalSum;
            PaymentRate = paymentRate;
        }

        public static ReportTotalInfo Default => new(0, 0, 0);
        
        public override string ToString() {
            return $"{TotalAmount} единиц на сумму {TotalSum.ToTwoDecimalPlaces()}$";
        }
    }
}