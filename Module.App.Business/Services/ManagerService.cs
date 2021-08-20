using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Module.App.Business.Interfaces;
using Module.App.Business.UnitOfWork;

namespace Module.App.Business.Services {
    public class ManagerService : IManagerService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ManagerService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<ManagerDetailsDTO>> GetManagersAsync() {
            var result = await _uow.ManagerDetails.All.Include(x => x.Manager).ToListAsync();
            return _mapper.Map<List<ManagerDetailsDTO>>(result);
        }

        public async Task<ManagerDetailsDTO> GetManagerAsync(int id) {
            var result = _mapper.Map<ManagerDetailsDTO>(await _uow.ManagerDetails.All.Include(x => x.Manager)
                .Include(x => x.Accesses).FirstAsync(x => x.ManagerId == id));
            return result;
        }

        public async Task UpdateManagerAsync(ManagerDetailsDTO managerDto) {
            await using (await _uow.BeginTransactionAsync()) {
                var manager = await _uow.ManagerDetails.GetByIdAsync(managerDto.Id);
                var isNew = false;
                if (manager == null) {
                    manager = new ManagerDetails {ManagerId = managerDto.Id};
                    isNew = true;
                }

                manager.RegistrationsQueueNumber = managerDto.RegistrationsQueueNumber;
                manager.NotificationsEmail = managerDto.NotificationsEmail;
                manager.SendNewOrderNotification = managerDto.SendNewOrderNotification;
                manager.SendNewRegistrationsNotification = managerDto.SendNewRegistrationsNotification;
                manager.RoleId = managerDto.RoleId;

                var accesses = managerDto.Accesses.Select(x => new ManagerAccess {
                    Id = Guid.NewGuid(),
                    AccessTo = x.AccessTo,
                    ManagerId = x.ManagerId,
                    AccessToManagerId = x.AccessToManagerId
                });

                if (isNew) {
                    await _uow.ManagerDetails.AddAsync(manager);
                    await _uow.ManagerAccess.AddRangeAsync(accesses);
                } else {
                    _uow.ManagerDetails.Update(manager);
                    _uow.ManagerAccess.DeleteRange(await _uow.ManagerAccess.All.Where(x => x.ManagerId == managerDto.Id)
                        .ToListAsync());
                    await _uow.ManagerAccess.AddRangeAsync(accesses);
                }

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }

            await _uow.BeginTransactionAsync();
        }

        public async Task<List<LookUpItem<int, string>>> GetManagersLookupListAsync() {
            return await _uow.Managers.All.Select(x => new LookUpItem<int, string>(x.Id, x.Name)).ToListAsync();
        }
    }
}