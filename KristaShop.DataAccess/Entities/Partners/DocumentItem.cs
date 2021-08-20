using System;
using System.Text.RegularExpressions;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.Partners;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="DocumentItemsConfiguration"/>
    /// </summary>
    public class DocumentItem : ICatalogItemBase, IEntityChangeLoggable {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid? FromDocumentId { get; set; }

        public Model Model { get; set; }
        public Color Color { get; set; }
        public Document Document { get; set; }
        public Document? FromDocument { get; set; }

        public string GetModelGroupingKey() {
            var size = Model == null ? Size.Value : Model.SizeLine;
            return Regex.Replace($"{Articul}_{size}", @"[!@#$%+\/^&*\s]", "_");
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }

    public class DocumentItemView : ICatalogItemBase {
        private SizeValue _size;
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string MainPhoto { get; set; }
        public string SizeLine { get; set; }

        public SizeValue Size {
            get => _size;
            set => _size = !value.Line.Equals(SizeLine) ? new SizeValue(value.Value, SizeLine) : value;
        }

        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public MovementDirection Direction { get; set; }
        public TimePeriodType Type { get; set; }
        
        public object Clone() {
            return MemberwiseClone();
        }
    }
}