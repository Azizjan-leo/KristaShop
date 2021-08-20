using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFor1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface INewUsersRepository : IRepository<NewUser, Guid> {
        Task<IEnumerable<NewUserSqlView>> GetNotApprovedAsync();
        Task<IEnumerable<NewUserSqlView>> GetGuestsAsync();
    }
}