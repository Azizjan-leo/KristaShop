using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Common.Business.Interfaces;
using Module.Common.Business.UnitOfWork;

namespace Module.Common.Business.Services {
    public class CityService : ICityService {
        private readonly ICommonUnitOfWork _uow;

        public CityService(ICommonUnitOfWork uow) {
            _uow = uow;
        }

        public async Task<List<LookUpItem<int, string>>> GetCitiesLookupListAsync() {
            return (await _uow.Cities.GetAllAsync()).Select(x => new LookUpItem<int, string>(x.Id, x.Name)).ToList();
        }
    }
}