using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Domain.Seeds.Common {
    public static class SeedExtension {
        internal static ModelBuilder Seed(this ModelBuilder builder, ISeeds seeds) {
            seeds.Seed(builder);
            return builder;
        }
    }
}
