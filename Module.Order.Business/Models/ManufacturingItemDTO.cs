using KristaShop.Common.Enums;

namespace Module.Order.Business.Models {
    public class ManufacturingItemDTO : BasicOrderItemDTO {
        public ManufactureState State { get; set; }

        public override string ItemStatus {
            get {
                switch (State) {
                    case ManufactureState.Kroy:
                        return "Крой";
                    case ManufactureState.KroyComplete:
                        return "Крой завершен";
                    case ManufactureState.Zapusk:
                        return "Запуск";
                    case ManufactureState.VPoshive:
                        return "В пошиве";
                    case ManufactureState.SkladGP:
                        return "На складе ГП";

                    default:
                        return "---";
                }
            }
        }
    }
}