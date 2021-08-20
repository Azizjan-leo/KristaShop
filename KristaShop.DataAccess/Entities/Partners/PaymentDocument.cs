using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="PaymentDocumentConfiguration"/>
    /// </summary>
    public class PaymentDocument : MoneyDocument {
        public override string Name => "К выплате поставщику";

        public PaymentDocument() {
            State = State.NotPaid;
        }

        public PaymentDocument(int userId, double paymentRate, ICollection<DocumentItem> items, State state) : base(userId, items) {
            Direction = MovementDirection.Out;
            State = state;
            foreach (var item in Items) {
                item.Price = paymentRate;
                item.PriceInRub = 0;
            }

            Sum = Items.Sum(x => x.Amount * x.Price);
        }
    }
}