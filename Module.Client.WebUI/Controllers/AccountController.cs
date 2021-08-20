using System;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.WebUI.Models;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Module.Common.WebUI.Models;
using Serilog;

namespace Module.Client.WebUI.Controllers {
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.FrontendScheme)]
    public class AccountController : AppControllerBase {
        private readonly ISignInService _signInService;
        private readonly ILinkService _linkService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(ISignInService signInService,
            ILinkService linkService, IMapper mapper, ILogger logger) {
            _signInService = signInService;
            _linkService = linkService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() {
            LoginViewModel model = new LoginViewModel();
            return PartialView("_LoginPartial", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                try {
                    var result = await _signInService.SignIn(model.UserName, model.Password, model.RememberMe, false);
                    if (!result.IsSuccess) {
                        ModelState.TryAddModelErrors(result.Messages);
                    } else {
                        SetNotification(result);
                        return StatusCode(StatusCodes.Status302Found, new RedirectViewModel("Index", "Home"));
                    }
                } catch (Exception ex) {
                    _logger.Error(ex, "Failed to login {@user}. {message}", model.UserName, ex.Message);
                    ModelState.TryAddModelErrors(OperationResult.Failure().Messages);
                }
            }

            return PartialView("_LoginPartial", model);
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LoginByLink(string code, string returnUrl) {
            try {
                var linkResult = await _linkService.GetUserIdByRandCodeAsync(code);
                if (linkResult.IsSuccess) {
                    await _signInService.SignOut(false);
                    var result = await _signInService.SignIn(linkResult.Model);

                    if (linkResult.Model.Type == AuthorizationLinkType.ChangePassword) {
                        returnUrl = Url.Action("ChangePassword", "Account");
                    }

                    SetNotification(result);
                    if (!result.IsSuccess) {
                        HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    } else {
                        if (!string.IsNullOrEmpty(returnUrl)) {
                            return LocalRedirect(returnUrl);
                        }
                    }
                } else {
                    SetNotification(linkResult.ToOperationResult());
                }

                return RedirectToAction("Index", "Home");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to login by link {link}. {message}", code, ex.Message);
                SetNotification(OperationResult.Failure());
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> TryLoginGuest(string guestAccessCode) {
            try {
                if (!string.IsNullOrEmpty(guestAccessCode) && _signInService.IsGuestAccessInfoCorrect(guestAccessCode)) {
                    var guestAccessInfo = _signInService.DecodeGuestAccessStr(guestAccessCode);
                    var principal = await _signInService.SignIn(guestAccessInfo);
                    if (principal != null) {
                        TempData["RedirectToCatalog"] = true;
                        return Redirect($"/Home/RedirectToCatalog?guestAccessCode={guestAccessCode}");
                    }
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to guest login by GuestCode {link}. {message}", guestAccessCode, ex.Message);
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Logout() {
            await _signInService.SignOut(false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromServices] ISettingsManager settingsManager, [FromServices] ICityService cityService) {
            try {
                if (!settingsManager.Settings.IsRegistrationActive) {
                    ViewData["InactiveRegistrationMessage"] = settingsManager.Settings.InactiveRegistrationMessage;
                    return PartialView("_InactiveRegistrationPartial");
                }

                var model = new RegisterViewModel();
                model.Cities = new SelectList(await cityService.GetCitiesLookupListAsync(), "Key", "Value");
                model.TermsOfUse = settingsManager.Settings.TermsOfUse;
                return PartialView("_RegisterPartial", model);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to load register partial view. {message}", ex.Message);
                return PartialView("_OpenFailurePartial", "Не удалось открыть форму регистрации");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, [FromServices] ISettingsManager settingsManager, [FromServices] ICityService cityService, [FromServices] IRegistrationService registrationService) {
            if (ModelState.IsValid) {
                try {
                    if (!settingsManager.Settings.IsRegistrationActive) {
                        ModelState.TryAddModelError(string.Empty, settingsManager.Settings.InactiveRegistrationMessage);
                        return StatusCode(StatusCodes.Status302Found, new RedirectViewModel("Index", "Home"));
                    }
                    var newUser = _mapper.Map<NewUserDTO>(model);
                    var result = await registrationService.RegisterAsync(newUser);
                    if (!result.IsSuccess) {
                        ModelState.TryAddModelErrors(result.Messages);
                    } else {
                        SetNotification(result);
                        return StatusCode(StatusCodes.Status302Found, new RedirectViewModel("Index", "Home"));
                    }
                } catch (Exception ex) {
                    _logger.Error(ex, "Failed to register {@user}. {message}", model, ex.Message);
                    ModelState.TryAddModelError(string.Empty, "Произошла ошибка во время регистрации.");
                }
            }

            model.Cities = new SelectList(await cityService.GetCitiesLookupListAsync(), "Key", "Value");
            return PartialView("_RegisterPartial", model);
        }

        public async Task<IActionResult> ChangePassword([FromServices] ILinkService linkService) {
            var model = new ChangePasswordViewModel {
                NeedCheckCurrentPassword = await _needCheckCurrentPasswordAsync(linkService)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passwordModel,
            [FromServices] IUserService userService, [FromServices] ILinkService linkService) {
            try {
                if (UserSession is UnauthorizedSession || UserSession.UserId <= 0)
                    return RedirectToAction("Common403", "Error");

                if (ModelState.IsValid) {
                    passwordModel.UserId = UserSession.UserId;

                    var passwordHash = HashHelper.TransformPassword(passwordModel.Password);

                    passwordModel.CurrentPassword = HashHelper.TransformPassword(passwordModel.CurrentPassword);

                    passwordModel.NeedCheckCurrentPassword = await _needCheckCurrentPasswordAsync(linkService);

                    if (passwordHash.Equals(passwordModel.CurrentPassword)) {
                        ModelState.TryAddModelError(string.Empty, "Текущий пароль и новый пароль не должны совпадать");

                        return View(passwordModel);
                    }

                    var checkCurrentPassword = !UserSession.Link.IsSignedInForChangePassword;

                    if (UserSession.Link.IsSignedInForChangePassword) {
                        if (!await linkService.IsLinkExistAsync(UserSession.Link.Code)) {
                            checkCurrentPassword = true;
                        }
                    }

                    if (checkCurrentPassword) {
                        var isPasswordValid =
                            await userService.ValidatePasswordAsync(UserSession.UserId, passwordModel.CurrentPassword);

                        if (!isPasswordValid) {
                            ModelState.TryAddModelError(nameof(ChangePasswordViewModel.CurrentPassword),
                                "Текущий пароль введен неверно");

                            return View(passwordModel);
                        }
                    }

                    var result = await userService.ChangeUserPasswordAsync(passwordModel.UserId, passwordHash,
                        passwordModel.Password);

                    if (result.IsSuccess) {
                        SetNotification(result);

                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.TryAddModelErrors(result.Messages);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to change user's password {@model} {message}", passwordModel, ex.Message);

                ModelState.AddModelError(string.Empty, "Произошла ошибка при измении пароля, повторите операцию позже.");
            }

            return View(passwordModel);
        }

        private async Task<bool> _needCheckCurrentPasswordAsync(ILinkService linkService) {
            var checkCurrentPassword = !UserSession.Link.IsSignedInForChangePassword;

            if (UserSession.Link.IsSignedInForChangePassword) {
                if (!await linkService.IsLinkExistAsync(UserSession.Link.Code)) {
                    checkCurrentPassword = true;
                }
            }

            return checkCurrentPassword;
        }
    }
}