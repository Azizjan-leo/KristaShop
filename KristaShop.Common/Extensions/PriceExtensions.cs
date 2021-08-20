using System;
using System.Globalization;

namespace KristaShop.Common.Extensions
{
    public static class PriceExtensions
    {
        public static string ToTwoDecimalPlaces(this double price)
        {
            var f = new NumberFormatInfo { NumberGroupSeparator = " ", CurrencyDecimalSeparator = "."};
            return price.ToString("n", f);
        }

        public static double ToDotSeparator(this double value) {
            var result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            return result;
        }
    }
}
