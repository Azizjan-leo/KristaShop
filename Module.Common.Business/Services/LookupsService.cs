using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Common.Business.UnitOfWork;

namespace Module.Common.Business.Services {
    public class LookupsService : ILookupsService {
        private readonly ICommonUnitOfWork _uow;

        public LookupsService(ICommonUnitOfWork uow) {
            _uow = uow;
        }

        public Task<List<LookUpItem<int, string>>> GetColorsLookupListAsync() {
            return _uow.Colors.All.Select(x => new LookUpItem<int, string>(x.Id, x.Name)).ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetSizesListAsync() {
            return await _uow.Models.GetAllSizesAsync();
        }

        public async Task<IReadOnlyList<string>> GetArticulsListAsync() {
            return await _uow.Models.GetAllArticulsAsync();
        }

        public async Task<IReadOnlyList<LookUpItem<int, string>>> GetManagersLookupListAsync() {
            return await _uow.Managers.All.Select(x => new LookUpItem<int, string>(x.Id, x.Name)).ToListAsync();
        }
    }
}
