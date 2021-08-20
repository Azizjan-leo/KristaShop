#nullable enable
using System;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="ColorsConfiguration"/>
    /// </summary>
    public class Color : IComparable<Color> {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? GroupId { get; set; }
        public ColorGroup? Group { get; set; }

        public int CompareTo(Color? other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            return idComparison != 0 ? idComparison : string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}
