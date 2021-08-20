using System.Collections.Generic;
using KristaShop.DataAccess.Domain.Seeds.Common;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Domain.Seeds {
    public class SequencesSeed : ISeeds {
        public ModelBuilder Seed(ModelBuilder builder) {
            builder.Entity<DocumentNumberSequence>().HasData(new List<DocumentNumberSequence> {new(1)});
            return builder;
        }
    }
}