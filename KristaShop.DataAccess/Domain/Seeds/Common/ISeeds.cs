using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Domain.Seeds.Common {
    internal interface ISeeds {
        ModelBuilder Seed(ModelBuilder builder);
    }
}
