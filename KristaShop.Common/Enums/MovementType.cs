using System.ComponentModel.DataAnnotations;

namespace KristaShop.Common.Enums {
    public enum MovementType {
        None = -1,
        [Display(Name = "Приход")]
        Income = 0,
        [Display(Name = "Списание")]
        WriteOff,
        [Display(Name = "Избыток")]
        IncomeAudit,
        [Display(Name = "Недостача")]
        WriteOffAudit,
        [Display(Name = "Продажа")]
        Selling,
        [Display(Name = "Возврат")]
        Return
    }

    public enum MovementDirection {
        None = -1,
        In = 0,
        Out
    }

    public static class MovementDirectionExtension {
        public static int GetMovementDirectionMultiplier(this MovementDirection direction) {
            if (direction == MovementDirection.Out) return -1;
            return 1;
        }
    }
}