using System;
using System.Linq;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Media {
    public class FaqRepository : Repository<Faq, Guid>, IFaqRepository<Faq, Guid>
    {
        public FaqRepository(DbContext context) : base(context) { }
        public override IOrderedQueryable<Faq> AllOrdered => All.Include(z=>z.FaqSections).OrderBy(x => x.Id);

        public Faq GetFaqByIdIncluding(Guid id)
        {
            return AllOrdered.FirstOrDefault(z => z.Id.Equals(id));
        }
    }
}
