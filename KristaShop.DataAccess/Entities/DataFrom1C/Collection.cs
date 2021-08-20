using System;
using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="CollectionConfiguration"/>
    /// </summary>
    public class Collection {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Date { get; set; }
        public int PercentValue { get; set; }
        public double Percent => (double)PercentValue / 100;

        public static Collection Default =>
            new() {
                Id = 0,
                Name = "Коллекция №1",
                Date = DateTime.Today,
                CreateDate = DateTime.Today,
                PercentValue = 30
            };
    }
}