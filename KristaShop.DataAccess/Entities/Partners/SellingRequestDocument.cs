using System.Collections.Generic;
using KristaShop.Common.Models;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.Partners {
    public class SellingRequestDocument : Document {
        public override string Name => "Запрос на реализацию";

        public SellingRequestDocument() {
            Items = new List<DocumentItem>();
        }
        
        public SellingRequestDocument(int userId, ICollection<DocumentItem> items) : base(userId, items) { }

        protected override StateChain GetStateChain() {
            return new(new List<StateChainItem> {
                new(State.Created, "Отправить", State.ApproveAwait),
                new(State.ApproveAwait, "Подтвердить", State.Approved),
            });
        }
    }
}