using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Media
{
    public class FaqSectionContentFileRepository : Repository<FaqSectionContentFile, Guid>, IFaqSectionContentFileRepository<FaqSectionContentFile, Guid>
    {
        public FaqSectionContentFileRepository(DbContext context) : base(context) { }
    }
}
