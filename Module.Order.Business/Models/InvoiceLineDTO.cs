using KristaShop.Common.Extensions;

namespace Module.Order.Business.Models {
    public class InvoiceLineDTO {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public bool IsProductLine { get; set; }

        public string Articul { get; set; }
        public string MainPhoto { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public string SizeValue { get; set; }
        public int Amount { get; set; }
        public int PartsCount { get; set; }

        public string Description { get; set; }

        public InvoiceCurrency Currency { get; set; }
        public double ExchangeRate { get; set; }

        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceInRub { get; set; }

        public string PriceFormated {
            get {
                if (Currency == InvoiceCurrency.USD) {
                    return $"{Price.ToTwoDecimalPlaces()} $";
                } else {
                    return $"{PriceInRub.ToTwoDecimalPlaces()} р";
                }
            }
        }

        public string TotalPriceFormated {
            get {
                if (Currency == InvoiceCurrency.USD) {
                    return $"{TotalPrice.ToTwoDecimalPlaces()} $";
                } else {
                    return $"{TotalPriceInRub.ToTwoDecimalPlaces()} р";
                }
            }
        }

        public void setParentInvoice(InvoiceDTO parentInvoice) {
            Currency = parentInvoice.Currency;
            ExchangeRate = parentInvoice.ExchangeRate;
            if (Currency == InvoiceCurrency.RUB) {
                PriceInRub = Price;
                Price = PriceInRub / (ExchangeRate <= 0.0d ? 1.0d : ExchangeRate);
            } else {
                PriceInRub = Price * ExchangeRate;
            }

            TotalPrice = Price * Amount;
            TotalPriceInRub = PriceInRub * Amount;
        }

    }
}
