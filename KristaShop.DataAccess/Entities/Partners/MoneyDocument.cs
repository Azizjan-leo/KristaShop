using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Configurations.Partners;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="IncomeDocumentConfiguration"/>
    /// </summary>
    public class MoneyDocument : Document {
        public override string Name => "Базовый денежный документ";
        public double Sum { get; set; }
        
        public MoneyDocument() { }

        public MoneyDocument(int userId, double sum) : base(userId, new List<DocumentItem>(), State.NotPaid) {
            Sum = sum;
        }

        public MoneyDocument(int userId, ICollection<DocumentItem> items) : base(userId, items, State.NotPaid) {
            Sum = items.Sum(x => x.Price * x.Amount);
        }

        protected override StateChain GetStateChain() {
            return new(new List<StateChainItem> {
                new(State.NotPaid, "Выставить счёт", State.PaymentAwait),
                new(State.PaymentAwait, "Подтвердить оплату", State.Paid),
            });
        }
    }
}