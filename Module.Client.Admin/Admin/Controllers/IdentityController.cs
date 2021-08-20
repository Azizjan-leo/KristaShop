using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using Serilog;
using System.Linq;
using KristaShop.Common.Extensions;
using Module.App.Business.Interfaces;
using Module.Client.Admin.Admin.Models;
using Module.Client.Admin.Admin.Models.Filters;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.Business.Services;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;

namespace Module.Client.Admin.Admin.Controllers {
    [Authorize(AuthenticationSchemes = "BackendScheme")]
    [Area("Admin")]
    public class IdentityController : AppControllerBase {
        private readonly IUserService _userService;
        private readonly ILinkService _linkService;
        private readonly IManagerService _managerService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public IdentityController(IUserService userService, ILinkService linkService, IManagerService managerService, IMapper mapper, ILogger logger) {
            _userService = userService;
            _linkService = linkService;
            _managerService = managerService;
            _mapper = mapper;
            _logger = logger;            
        }

        public async Task<IActionResult> Index(UsersFilter filter) {
            try {
                ViewBag.ManagersList = new SelectList(await _managerService.GetManagersLookupListAsync(), "Value", "Value");
                return View(await _userService.GetUsersAsync(UserSession, true));
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load users list. {message}", ex.Message);
                return Problem(OperationResult.FailureAjax("Возникла ошибка при получении списка пользователей"));
            }
           
        }
        
        public async Task<IActionResult> ShowCart(int id, [FromServices] SignInService signInService) {
            await signInService.SignOut(false);
            await signInService.SignOutGuest();

            var hash = HashHelper.TransformPassword(id.ToString());
            return Redirect($"/Cart/ShowGuestCart?userId={id}&hash={hash}");
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(int userId) { //, [FromServices] IClientReportService reportService TODO: report service
            try {
                var result = await _linkService.InsertLinkAuthAsync(userId);
                if (result.IsSuccess) {

                    // var orderData = await reportService.GetUserComplexOrderAsync(userId);
                    // var unprocessed = await reportService.GetUserUnprocessedItemsAsync(userId);
                    var extraLinks = new List<object> {
                        new {DocName = "Личный кабинет", Link = $"{result.Model}&action=Index"}
                    };
                    
                    // if (orderData.HasAnyItems()) {
                    //     if (unprocessed.Items.Any()) {
                    //         extraLinks.Add(new { DocName = "Необработанные заказы", Link = $"{result.Model}&action=Index" });
                    //     }
                    //
                    //     if (orderData.ProcessedItems.Items.Any()) {
                    //         extraLinks.Add(new {DocName = "Заказы в обработке", Link = $"{result.Model}&action=ProcessingOrder"});
                    //     }
                    //
                    //     if (orderData.Shipments.Items.Any()) {
                    //         extraLinks.Add(new {DocName = "История заявок", Link = $"{result.Model}&action=OrdersHistory" });
                    //         extraLinks.Add(new { DocName = "История отправок", Link = $"{result.Model}&action=ShipmentDetails"});
                    //     }
                    // }

                    extraLinks.Add(new { DocName = "Каталог", Link = $"{result.Model}&action=Index&controller=Catalog" });

                    return Ok(new { link = result.Model, docLinks = (extraLinks.Count > 0 ? extraLinks.ToArray() : null) });
                }

                _logger.Warning(result.Exception, "{message}. {@result}", result.ToString(), result);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to create login link. {message}", ex.Message);
            }

            return BadRequest("Не удалось сгенерировать ссылку для смены пароля");
        }


        public async Task<IActionResult> OpenUserCart(int userId)
        {
            try {
                var result = await _linkService.InsertLinkAuthAsync(userId, AuthorizationLinkType.MultipleAccess, false);
                if (result.IsSuccess) {
                    return LocalRedirect($"/Home?{result.Model}&redirect=/Cart");
                }
                return BadRequest($"Не удалось открыть корзину пользователя. {result.ReadableMessage}");
            } catch (Exception ex)  {
                _logger.Error(ex, "Failed to open user's cart. {message}", ex.Message);
                return BadRequest("Не удалось открыть корзину пользователя");
            }
        }

        public async Task<IActionResult> CreateChangePasswordLink(int userId) {
            try {
                var result = await _linkService.InsertLinkAuthAsync(userId, AuthorizationLinkType.ChangePassword);
                if (!result.IsSuccess) {
                    return BadRequest($"Не удалось сгенерировать ссылку для смены пароля. {result.ReadableMessage}");
                }

                return Ok(new { link = result.Model });
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to generate change password link. {message}", ex.Message);
            }

            return BadRequest("Не удалось сгенерировать ссылку для смены пароля");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLink(int userId) {
            try {
                var result = await _linkService.RemoveLinksByUserIdAsync(userId);
                if (!result.IsSuccess) {
                    _logger.Warning(result.Exception, "{message}. {@result}", result.ToString(), result);
                }

                return Ok(result.ToOperationResult());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to remove user {userId} links. {message}", userId, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось удалить ссылки пользователя"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNewUserRequest(Guid id, [FromServices] IRegistrationService registrationService) {
            try {
                var result = await registrationService.DeleteRequestAsync(id);
                SetNotification(result);
                return Ok(new object());
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to remove new user request (id: {id}). {message}", id, ex.Message);
                return BadRequest(OperationResult.Failure("Не удалось удалить заявку на регистрацию нового пользователя"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GuestAccessLink() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuestAccessLink(GuestAccessLinkModel model, [FromServices] ISignInService signInService) {
            ViewBag.GuestLink = string.Empty;

            if (!model.IsPreoderVisible && !model.IsInstockByLinesVisible && !model.IsInstockByPartsVisible) {
                ModelState.AddModelError("", "Необходимо указать хотя бы один каталог");
            }
            if (model.ExpiredDate < DateTime.Now) {
                ModelState.AddModelError("ExpiredDate", "Необходимо указать корректную дату окончания действия ссылки");
            }

            if (ModelState.IsValid) {
                ViewBag.GuestLink = signInService.GetGuestLink(model.ExpiredDate, model.IsPreoderVisible, model.IsInstockByLinesVisible, model.IsInstockByPartsVisible);
            }

            return View();
        }

        [NonAction]
        private string _generatePass(int len = 6) {
            var result = string.Empty;
            if (len <= 0) return result;

            var random = new Random(DateTime.Now.Millisecond);

            while (result.Length < len) {
                int rndDigit = random.Next(0, 10);
                result += rndDigit.ToString();
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser(Guid id, string from, [FromServices] ICityService cityService, [FromServices] IRegistrationService registrationService) {
            UserClientDTO newUser;
            try {
                newUser = await registrationService.GetRequestAsync(id);
                if (newUser == null) {
                    SetNotification(OperationResult.Failure("Запись не найдена в БД"));
                    return RedirectToAction("Index", "Home");
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to approve user");
                SetNotification(OperationResult.Failure("Запись не найдена в БД"));
                return RedirectToAction("Index", "Home");
            }

            var allCatalogs = CatalogTypeExtensions.GetAllCatalogsExceptOpen()
                .Select(x => new {Id = (int) x, Name = x.GetDisplayName()})
                .ToList();

            if (string.IsNullOrEmpty(from)) from = "home";

            var model = new NewUserViewModel() {
                Id = id,
                UserId = newUser.UserId,
                FullName = newUser.FullName,
                Login = newUser.Login,
                Password = _generatePass(),
                Phone = newUser.Phone,
                Email = newUser.Email,
                ManagerId = newUser.ManagerId,
                Managers = new SelectList(await _managerService.GetManagersLookupListAsync(), "Key", "Value"),
                CityId = newUser.CityId,
                Cities = new SelectList(await cityService.GetCitiesLookupListAsync(), "Key", "Value"),
                OtherCity = newUser.NewCity,
                MallAddress = newUser.MallAddress,
                CompanyAddress = newUser.CompanyAddress,
                CatalogsList = new SelectList(allCatalogs, "Id", "Name"),
                ReturnTo = from.Trim().ToLower()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(NewUserViewModel model, [FromServices] ICityService cityService, [FromServices] IRegistrationService registrationService) {
            if (ModelState.IsValid) {
                bool hasError = false;

                if (string.IsNullOrEmpty(model.OtherCity)) {
                    if (model.CityId == null || model.CityId <= 0) {
                        ModelState.AddModelError("CityId", "Укажите город контрагента");
                        hasError = true;
                    }
                } else {
                    if (model.CityId != null && model.CityId > 0) {
                        model.OtherCity = string.Empty;
                    }
                }

                if (await _userService.IsLoginExistsAsync(model.Login)) {
                    ModelState.AddModelError("Login", "Контрагент с таким логином уже существует");
                    hasError = true;
                }

                if (!hasError) {
                    var result = await registrationService.ApproveRequestAsync(_mapper.Map<NewUserDTO>(model), model.VisibleCatalogs, model.SendToEmail);
                    SetNotification(result);
                    if (model.ReturnTo == "home") {
                        return RedirectToAction("Index", "Home");
                    } else if (model.ReturnTo == "users") {
                        return RedirectToAction("Index", "Identity");
                    } else {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            var allCatalogs = CatalogTypeExtensions.GetAllCatalogsExceptOpen()
                .Select(x => new {Id = (int) x, Name = x.GetDisplayName()})
                .ToList();

            model.Managers = new SelectList(await _managerService.GetManagersLookupListAsync(), "Key", "Value");
            model.Cities = new SelectList(await cityService.GetCitiesLookupListAsync(), "Key", "Value");
            model.CatalogsList = new SelectList(allCatalogs, "Id", "Name");

            return View(model);
        }
    }
}