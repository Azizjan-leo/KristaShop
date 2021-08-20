using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Entities.DataFor1C;
using Microsoft.EntityFrameworkCore;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.Business.UnitOfWork;

namespace Module.Client.Business.Services {
    public class UserService : IUserService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        public async Task<List<UserClientDTO>> GetUsersAsync(UserSession user, bool includeNew = false) {
            List<UserClientDTO> result;
            if (!user.IsRoot) {
                var managerIds = await _uow.ManagerAccess
                    .GetManagerIdsAccessesFor(user.ManagerId, ManagerAccessToType.Users)
                    .ToListAsync();
                result = _mapper.Map<List<UserClientDTO>>(await _uow.Users.GetAllByManagersAsync(managerIds));
            } else {
                result = _mapper.Map<List<UserClientDTO>>(await _uow.Users.GetAllAsync());
            }
            
            if (includeNew) {
                result.AddRange(_mapper.Map<List<UserClientDTO>>(await _uow.NewUsers.GetNotApprovedAsync()));
            }

            var usersWithCart = (await _uow.Carts.GetUserIdsWithNotEmptyCarts()).ToDictionary(k => k, v => true);
            foreach (var client in result) {
                if (usersWithCart.ContainsKey(client.UserId)) {
                    client.CartStatus = true;
                }
            }

            return result;
        }
        
        public async Task<UserClientDTO> GetUserAsync(int id) {
            return _mapper.Map<UserClientDTO>(await _uow.Users.GetByIdAsync(id));
        }

        public async Task<UserClientDTO> GetUserAsync(string login) {
            return _mapper.Map<UserClientDTO>(await _uow.Users.GetByLoginAsync(login));
        }

        public async Task<UserSession> GetUserDetailsAsync(UserSession session) {
            if (session.IsRoot) {
                return session;
            }
            
            if (session.IsManager) {
                return _mapper.Map(await _uow.Users.GetManagerAsync(session.UserId), session);
            }
            
            var result = _mapper.Map(await _uow.Users.GetByIdAsync(session.UserId), session);
            result.IsPartner = await _uow.Partners.IsPartnerAsync(session.UserId);
            return result;
        }

        public async Task<bool> IsActiveUserAsync(int userId) {
            return await _uow.Users.IsActiveAsync(userId);
        }

        public async Task<bool> IsLoginExistsAsync(string login) {
            return await _uow.Users.IsAlreadyRegisteredAsync(login);
        }
        
        public async Task<bool> ValidatePasswordAsync(int userId, string passwordHash) {
            var user = await _uow.Users.GetByIdAsync(userId);
            return user.Password.Equals(passwordHash) || await _isNewPasswordValidAsync(userId, passwordHash);
        }

        private async Task<bool> _isNewPasswordValidAsync(int userId, string pass) {
            return await _uow.UserNewPasswords.All.Where(x => x.UserId == userId && x.Password == pass).AnyAsync();
        }
 
        public async Task<OperationResult> ChangeUserPasswordAsync(int userId, string newPasswordHash, string newPasswordSrc) {
            using (_uow.BeginTransactionAsync()) {
                var newPassword = await _uow.UserNewPasswords.GetByIdAsync(userId);
                if (newPassword == null) {
                    newPassword = new UserNewPassword(userId, newPasswordHash, newPasswordSrc);
                    await _uow.UserNewPasswords.AddAsync(newPassword);
                } else {
                    newPassword.Password = newPasswordHash;
                    newPassword.PasswordSrc = newPasswordSrc;
                    _uow.UserNewPasswords.Update(newPassword);
                }

                var user = await _uow.Users.GetByIdAsync(userId);
                user.Password = newPasswordHash;
                _uow.Users.Update(user);

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
                return OperationResult.Success("Пароль успешно изменен");
            }
        }
    }
}