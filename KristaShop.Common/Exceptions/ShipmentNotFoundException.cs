using System;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Exceptions {
    public class ShipmentNotFoundException : EntityNotFoundException {
        public ShipmentNotFoundException(DateTime date, int userId)
            : base($"Shipment of date {date.ToBasicString()} not found or already incomed for user {userId}",
                $"Отправка от даты {date.ToBasicString()} не найдена или уже оприходована") { }
    }
}