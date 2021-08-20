using System.Collections.Generic;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.MaterialConfiguration"/>
    /// </summary>
    public class Material {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<Model> Models { get; set; }
    }
}