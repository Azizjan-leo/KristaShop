using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KristaShop.DataAccess.Entities
{
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Favorite1CItemConfiguration"/>
    /// </summary>

    public class Favorite1CItemItem {
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int CatalogId { get; set; }
    }
}