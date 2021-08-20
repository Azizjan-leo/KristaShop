using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="DocumentConfiguration"/>
    /// </summary>
    public class Document : IEntityKeyGeneratable, IEntityChangeLoggable {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public ulong Number { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int UserId { get; set; }
        public string DocumentType { get; set; }
        public virtual string Name => "Базовый документ";
        public MovementDirection Direction { get; set; }
        public State State { get; set; }
        public Document Parent { get; set; }
        public ICollection<Document> Children { get; set; }
        public ICollection<DocumentItem> Items { get; set; }
        public Partner Partner { get; set; }
        public Document() { }

        public Document(int userId, ICollection<DocumentItem> items, State state = State.Created) {
            DocumentType = GetType().Name;
            UserId = userId;
            CreateDate = DateTimeOffset.Now;
            Direction = MovementDirection.None;
            foreach (var item in items) {
                item.Id = Guid.NewGuid();
                item.FromDocument = item.Document;
                item.Document = this;
                if (item.Date == default) {
                    item.Date = CreateDate;
                }
            }

            Items = items;
            Children = new List<Document>();
            State = state;
        }
        
        public Document(int userId, ICollection<DocumentItem> items, ICollection<Document> children, State state = State.Created) {
            DocumentType = GetType().Name;
            UserId = userId;
            CreateDate = DateTimeOffset.Now;
            Direction = MovementDirection.None;
            foreach (var item in items) {
                item.Id = Guid.NewGuid();
                item.Document = this;
                if (item.Date == default) {
                    item.Date = CreateDate;
                }
            }
            Items = items;

            foreach (var child in children) {
                child.Parent = this;
            }

            Children = children;
            State = state;
        }
        
        protected virtual StateChain GetStateChain() {
            return new(new List<StateChainItem> {
                new(State.Created, "Подтвердить", State.Completed),
            });
        }

        public void SetNextState() {
            State = GetStateChain().GetNextState(State);
        }

        public string GetNextActionName() {
            return GetStateChain().GetNextAction(State);
        }
        
        public void GenerateKey() {
            Id = Guid.NewGuid();
        }

        public ReportTotalInfo GetTotals() {
            if (!Items.Any()) {
                return ReportTotalInfo.Default;
            }
            
            return new(Items.Sum(x => x.Amount), Items.Sum(x => x.Price * x.Amount), Items.Sum(x => x.PriceInRub * x.Amount));
        }
    }
}