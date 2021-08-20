using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.InvoiceLineSqlConfiguration"/>
    /// </summary>
    public class InvoiceLineSql {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string Size { get; set; }
        public string SizeLine { get; set; }
        public string Description { get; set; }
        public bool IsProductLine { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public string Articul { get; set; }
        public int PartsCount { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorGroup { get; set; }
        public string ColorValue { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
    }
}
