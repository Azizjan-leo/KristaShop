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
    public class FaqSectionRepository : Repository<FaqSection, Guid>, IFaqSectionRepository<FaqSection, Guid>
    {
        public FaqSectionRepository(DbContext context) : base(context) { }
        public override IOrderedQueryable<FaqSection> AllOrdered => All.Include(z => z.FaqSectionContents).OrderBy(x => x.Id);

        public FaqSection GetFaqSectionByIdIncluding(Guid id)
        {
            return AllOrdered.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<List<FaqSection>> GetFaqSections(Guid id)
        {
            return await AllOrdered.Where(z => z.FaqId.Equals(id)).Include(z=>z.FaqSectionContents).ThenInclude(z=>z.FaqSectionContentFiles).ToListAsync();
        }
    }
}
