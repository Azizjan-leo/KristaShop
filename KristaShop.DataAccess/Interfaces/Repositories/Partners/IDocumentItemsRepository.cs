using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IDocumentItemsRepository : IRepository<DocumentItem, Guid> {
        
    }
}