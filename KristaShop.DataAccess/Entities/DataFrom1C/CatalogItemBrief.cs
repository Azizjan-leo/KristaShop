using System;
using System.Collections.Generic;
using System.Text;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="CatalogItemBriefConfiguration"/>
    /// </summary>
    public class CatalogItemBrief {
        protected bool Equals(CatalogItemBrief other) {
            return Articul == other.Articul &&
                   IsVisible == other.IsVisible &&
                   CatalogId == other.CatalogId &&
                   // Price.Equals(other.Price) &&
                   // PriceInRub.Equals(other.PriceInRub) &&
                   Order == other.Order &&
                   MainPhoto == other.MainPhoto &&
                   AltText == other.AltText &&
                   // Nullable.Equals(AddDate, other.AddDate) &&
                   ItemsCount == other.ItemsCount &&
                   // Description == other.Description &&
                   IsLimited == other.IsLimited;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CatalogItemBrief) obj);
        }

        public override int GetHashCode() {
            var hashCode = new HashCode();
            hashCode.Add(Articul);
            hashCode.Add(IsVisible);
            hashCode.Add(CatalogId);
            hashCode.Add(Price);
            hashCode.Add(PriceInRub);
            hashCode.Add(Order);
            hashCode.Add(MainPhoto);
            hashCode.Add(AltText);
            hashCode.Add(AddDate);
            hashCode.Add(ItemsCount);
            hashCode.Add(Description);
            hashCode.Add(IsLimited);
            return hashCode.ToHashCode();
        }

        public string Articul { get; set; }
        public bool IsVisible { get; set; }
        public int CatalogId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }       
        public int Order { get; set; }
        public string MainPhoto { get; set; }
        public string AltText { get; set; }
        public DateTime? AddDate { get; set; }
        public int ItemsCount { get; set; }
        public string Description { get; set; }
        public bool IsLimited { get; set; }
    }
}
