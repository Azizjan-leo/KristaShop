using KristaShop.Common.Attributes;

namespace KristaShop.Common.Enums {
    public enum State {
        [DocumentState(Name = "")]
        None = -1,
        
        [DocumentState(Name = "Создан")]
        Created,

        [DocumentState(Name = "Ожидает подтверждения", HighlightColor = "warning")]
        ApproveAwait,

        [DocumentState(Name = "Подтвержден", HighlightColor = "success")]
        Approved,

        [DocumentState(Name = "Завершен", HighlightColor = "success")]
        Completed,

        [DocumentState(Name = "Не оплачен", HighlightColor = "danger")] // еще не выставленный счет
        NotPaid,

        [DocumentState(Name = "Ожидание", HighlightColor = "warning")]
        PaymentAwait,

        [DocumentState(Name = "Оплачен", HighlightColor = "success")]
        Paid
    }
}