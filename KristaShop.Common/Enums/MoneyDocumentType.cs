using System.ComponentModel.DataAnnotations;

namespace KristaShop.Common.Enums {
    public enum MoneyDocumentType {
        None = -1,
        [Display(Name = "Отправка", Description = "Реализация")]
        Selling = -2134267933, // HashCode.Combine("Реализация")
        [Display(Name = "Оплата", Description = "Оплата")]
        CashReceiptNote = -1678523933, // HashCode.Combine("Оплата")
        [Display(Name = "Ввод Остатков Покупателя", Description = "Ввод Остатков Покупателя")]
        InputRests = -441663038, // HashCode.Combine("ВводОстатковПокупателя")
    }
}