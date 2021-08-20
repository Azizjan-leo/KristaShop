using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Models;
using Module.Common.Business.UnitOfWork;

namespace Module.Common.Business.Services {
    public class SettingsService : ISettingsService {
        private readonly ICommonUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ISettingsManager _settingsManager;

        public SettingsService(ICommonUnitOfWork uow, IMapper mapper,
            ISettingsManager settingsManager) {
            _uow = uow;
            _mapper = mapper;
            _settingsManager = settingsManager;
        }

        public async Task<List<SettingsDTO>> GetSettingsAsync(bool isRoot) {
            return await _uow.Settings.All
                  .Where(x => x.OnlyRootAccess == isRoot)
                  .OrderBy(x => x.Key)
                  .ProjectTo<SettingsDTO>(_mapper.ConfigurationProvider)
                  .ToListAsync();
        }

        public async Task<SettingsDTO> GetByIdAsync(Guid id) {
            var entity = await _uow.Settings.GetByIdAsync(id);
            return _mapper.Map<SettingsDTO>(entity);
        }

        public async Task<SettingsDTO> GetByKeyAsync(string key) {
            var entity = await _uow.Settings.All.FirstAsync(x => x.Key == key);
            return _mapper.Map<SettingsDTO>(entity);
        }

        public async Task<OperationResult> InsertAsync(SettingsDTO setting) {
            var entity = _mapper.Map<Settings>(setting);

            await _uow.Settings.AddAsync(entity, true);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            await _settingsManager.ReloadAsync(entity.Id);
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateAsync(SettingsDTO setting, bool canChangeKey) {
            var entity = await _uow.Settings.GetByIdAsync(setting.Id);
            if (entity == null) {
                return OperationResult.Failure("Настройки с таким ключем не существует");
            }

            entity.Value = setting.Value;
            entity.Description = setting.Description;
            if (canChangeKey) {
                entity.Key = setting.Key;
            }

            _uow.Settings.Update(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            await _settingsManager.ReloadAsync(entity.Id);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteAsync(Guid id) {
            var entity = await _uow.Settings.GetByIdAsync(id);

            await _uow.Settings.DeleteAsync(id);

            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            await _settingsManager.ReloadAsync(id, (entity != null ? entity.Key : string.Empty));

            return OperationResult.Success();
        }
    }
}