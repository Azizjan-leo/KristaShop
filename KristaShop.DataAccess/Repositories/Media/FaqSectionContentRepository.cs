using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Media
{
    public class FaqSectionContentRepository : Repository<FaqSectionContent, Guid>, IFaqSectionContentRepository<FaqSectionContent, Guid>
    {
        public FaqSectionContentRepository(DbContext context) : base(context) { }

        public async Task<List<FaqSectionContent>> GetFaqSectionContent(Guid id)
        {
            return await All.Where(x => x.FaqSectionId.Equals(id)).Include(z=>z.FaqSectionContentFiles).ToListAsync();
        }
    }
}
