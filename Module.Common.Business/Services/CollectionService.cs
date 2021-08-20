using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Models;
using Module.Common.Business.UnitOfWork;

namespace Module.Common.Business.Services {
    public class CollectionService : ICollectionService {
        private readonly ICommonUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CollectionService(ICommonUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<LookUpItem<int, string>>> GetCollectionsLookupListAsync() {
            return await _uow.Collections.All
                .Select(x => new LookUpItem<int, string>(x.Id, x.Name))
                .ToListAsync();
        }

        public async Task<PrepayValue> GetCurrentPrepayPercentAsync() {
            var collection = await _uow.Collections.GetCurrentCollectionAsync();
            return new PrepayValue(collection.PercentValue.ToString(), collection.Percent);
        }

        public async Task<CollectionDTO> GetLastCollectionAsync() {
            var lastCollection = await _uow.Collections.All.OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            return _mapper.Map<CollectionDTO>(lastCollection);
        }
    }
}