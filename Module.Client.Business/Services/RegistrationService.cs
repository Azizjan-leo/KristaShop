using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.Business.UnitOfWork;
using Module.Common.Business.Interfaces;
using Serilog;

namespace Module.Client.Business.Services {
    public class RegistrationService : IRegistrationService {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly GlobalSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RegistrationService(IUnitOfWork uow, IOptions<GlobalSettings> settings,
            IEmailService emailService, IMapper mapper, ILogger logger) {
            _uow = uow;
            _emailService = emailService;
            _settings = settings.Value;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult> RegisterAsync(NewUserDTO newUser) {
            City city = new();
            if (newUser.CityId.HasValue) {
                city = await _uow.Cities.GetByIdAsync(newUser.CityId.Value);
                if (city == null) {
                    return OperationResult.Failure("Выбранный город не найден.");
                }
            }

            var entity = _mapper.Map<NewUser>(newUser);
            entity.Login = entity.CreateLoginFromName(true);

            if (_uow.NewUsers.All.Any(x => x.Login == entity.Login)) {
                return OperationResult.Success(new List<string>
                    {"Вы успешно зарегестрировались на сайте.", "Ожидайте подтверждения менеджера."});
            }

            entity.Password = HashHelper.TransformPassword(Generator.NewString(10));
            entity.ManagerId = await _getManagerIdForNewUserAsync();
            entity.Login = entity.CreateLoginFromName();

            if (_uow.NewUsers.All.Any(x => x.Login.Equals(entity.Login))) {
                entity.Login = entity.CreateLoginFromName(true);
            }

            if (_uow.NewUsers.All.Any(x => x.Login.Equals(entity.Login))) {
                return OperationResult.Success(new List<string>
                    {"Вы успешно зарегестрировались на сайте.", "Ожидайте подтверждения менеджера."});
            }

            if (await _uow.Users.IsAlreadyRegisteredAsync(entity.Login, entity.Phone)) {
                return OperationResult.Failure(new List<string> {
                    "Пользователь с указанными данными уже существует.",
                    "Обратитесь к менеджеру для получения доступа к сайту."
                });
            }

            await using (await _uow.BeginTransactionAsync()) {
                await _uow.NewUsers.AddAsync(entity, true);
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }

            await _sendEmailToManagerAsync(entity, city);

            return OperationResult.Success(new List<string>
                {"Вы успешно зарегестрировались на сайте.", "Ожидайте подтверждения менеджера."});
        }
        
        public async Task<List<NewUserDTO>> GetRequestsAsync() {
            return _mapper.Map<List<NewUserDTO>>(await _uow.NewUsers.GetNotApprovedAsync());
        }

        public async Task<List<NewUserDTO>> GetGuestsRequestsAsync() {
            return _mapper.Map<List<NewUserDTO>>(await _uow.NewUsers.GetGuestsAsync());
        }
        
        public async Task<int> GetRequestsCountAsync(int managerId) {
            var managerIds = (await _uow.ManagerAccess.GetManagerIdsAccessesFor(managerId, ManagerAccessToType.Users).ToListAsync()).Cast<int?>();
            return await _uow.NewUsers.All
                .Where(x => managerIds.Contains(x.ManagerId) && x.UserId == null || x.UserId < 0)
                .CountAsync();
        }

        public async Task<UserClientDTO> GetRequestAsync(Guid id) {
            return _mapper.Map<UserClientDTO>(await _uow.NewUsers.GetByIdAsync(id));
        }

        public async Task<int> GetRequestsCountAsync() {
            return await _uow.NewUsers.All
                .Where(x => x.UserId == null || x.UserId < 0)
                .CountAsync();
        }

        public async Task<OperationResult> DeleteRequestAsync(Guid id) {
            var newUser = await _uow.NewUsers.GetByIdAsync(id);
            if (newUser == null) {
                return OperationResult.Failure("Заявка не найдена в БД");
            }

            await using (await _uow.BeginTransactionAsync()) {
                if (newUser.UserId is < 0) {
                    await _uow.Carts.ClearUserCartAsync(newUser.UserId.Value);
                }

                await _uow.NewUsers.DeleteAsync(id);
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }

            return OperationResult.Success("Заявка на регистрацию нового пользователя успешно удалена");
        }

        public async Task<OperationResult> ApproveRequestAsync(NewUserDTO newUserDTO, IEnumerable<CatalogType> visibleCatalogs,
            bool sendEmailToNewUser) {
            var catalogs = visibleCatalogs.ToDictionary(k => k, v => true);

            await using (await _uow.BeginTransactionAsync()) {
                var user = new User {
                    Name = newUserDTO.FullName,
                    FullName = newUserDTO.FullName,
                    Login = newUserDTO.Login,
                    Phone = newUserDTO.Phone ?? string.Empty,
                    Email = newUserDTO.Email ?? string.Empty,
                    Address = newUserDTO.CompanyAddress,
                    MallAddress = newUserDTO.MallAddress,
                    CityId = newUserDTO.CityId ?? 0,
                    IsManager = false,
                    ManagerId = newUserDTO.ManagerId,
                    CreateDate = newUserDTO.CreateDate ?? DateTime.Now,
                };
                user.SetPassword(newUserDTO.Password);
                user.SetCatalogsAccesses(catalogs);
                await _uow.Users.AddAsync(user, true);

                var request = await _uow.NewUsers.GetByIdAsync(newUserDTO.Id);
                request.FullName = newUserDTO.FullName;
                request.CityId = newUserDTO.CityId ?? 0;
                request.NewCity = newUserDTO.NewCity;
                request.MallAddress = newUserDTO.MallAddress ?? string.Empty;
                request.CompanyAddress = newUserDTO.CompanyAddress;
                request.Phone = newUserDTO.Phone ?? string.Empty;
                request.Email = newUserDTO.Email ?? string.Empty;
                request.Login = newUserDTO.Login;
                request.Password = HashHelper.TransformPassword(newUserDTO.Password);
                request.ManagerId = newUserDTO.ManagerId;
                request.UserId = newUserDTO.UserId;
                _uow.NewUsers.Update(request);

                if (newUserDTO.UserId < 0) {
                    await _uow.Carts.MoveCartItemsToOtherUserAsync(newUserDTO.UserId.Value, user.Id);
                }

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }

            if (sendEmailToNewUser) {
                await _sendEmailToNewUserAsync(newUserDTO);
            }

            return OperationResult.Success("Контрагент успешно создан");
        }

        private async Task<int> _getManagerIdForNewUserAsync() {
            var managers = await _uow.ManagerDetails.All.Where(x => x.RegistrationsQueueNumber > 0).ToListAsync();

            if (!managers.Any()) {
                return 0;
            }

            var newUsersCounter = await _getNewUsersCounterAsync(managers.Count);
            var manager = managers.FirstOrDefault(x => x.RegistrationsQueueNumber == newUsersCounter);
            return manager?.ManagerId ?? 0;
        }

        private async Task<long> _getNewUsersCounterAsync(int maxCount) {
            var counter = await _uow.NewUsersCounter.All.FirstOrDefaultAsync();
            if (counter == null) {
                counter = new NewUsersCounter(1);
                await _uow.NewUsersCounter.AddAsync(counter, true);
            } else {
                if (counter.Counter >= maxCount) {
                    counter.ResetCounter();
                } else {
                    counter.IncreaseCounter();
                }

                _uow.NewUsersCounter.Update(counter);
            }

            await _uow.SaveChangesAsync();
            return counter.Counter;
        }
        
        private async Task _sendEmailToManagerAsync(NewUser newUser, City city) {
            try {
                var managers = await _uow.ManagerDetails.All.Include(x => x.Manager).Include(x => x.Accesses)
                    .Where(x => x.ManagerId == newUser.ManagerId || (x.SendNewRegistrationsNotification == true &&
                                                                     x.Accesses.Any(a =>
                                                                         a.AccessToManagerId == newUser.ManagerId)))
                    .GroupBy(x => x.NotificationsEmail, x => new {x.Manager.Name})
                    .Select(x => new {Email = x.Key, Name = x.Min(c => c.Name)}).ToListAsync();

                if (!managers.Any()) {
                    managers.Add(new {Email = _settings.DefaultManagerEmail, Name = "Менеджер"});
                }

                var content =
                    $"Пожалуйста, добавьте пользователя! <br/>Имя: <b>{newUser.FullName}</b> <br/>Tел: <b><a href=\"https://api.whatsapp.com/send?phone={newUser.Phone}\">{newUser.Phone}</a></b> " +
                    $"<br/>Email: <b><a href=\"mailto:{newUser.Email}\">{newUser.Email}</a></b>" +
                    $" <br/>Город: <b>{city.Name}</b> <br/>Магазин: <b>{newUser.MallAddress}</b><br/>Адрес: <b>{newUser.CompanyAddress}</b>";

                foreach (var manager in managers) {
                    var subject = $"Уважаемый(ая) {manager.Name}";
                    var emailMessage = new EmailMessage(manager.Email, subject, content);

                    await _emailService.SendEmailAsync(emailMessage, manager.Name);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to send order email to manager. {message}", ex.Message);
            }
        }
        
        private async Task _sendEmailToNewUserAsync(NewUserDTO newUserDTO) {
            try {
                var userEmail = newUserDTO.Email;
                if (string.IsNullOrEmpty(userEmail)) return;

                var subject = $"Одобрена заявка на регистрацию на сайте krista.fashion";
                var content = $@"Ваша заявка на регистрацию на сайте krista.fashion одобрена! 
                                Ваш логин: {newUserDTO.Login} 
                                Ваш пароль: {newUserDTO.Password}";
                var emailMessage = new EmailMessage(userEmail, subject, content);
                await _emailService.SendEmailAsync(emailMessage, newUserDTO.FullName);
            } catch (Exception) {

            }
        }
    }
}