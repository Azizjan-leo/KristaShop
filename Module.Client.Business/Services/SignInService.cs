using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.Business.UnitOfWork;
using Serilog;

namespace Module.Client.Business.Services {
    public class SignInService : ISignInService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly KristaShopDbContext _context;
        private readonly GlobalSettings _settings;
        private readonly UrlSetting _urlSettings;

        public SignInService(IUnitOfWork uow, IMapper mapper, ILogger logger, IOptions<GlobalSettings> settings, IHttpContextAccessor httpContextAccessor, KristaShopDbContext context, IOptions<UrlSetting> urlOptions) {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _settings = settings.Value;
            _urlSettings = urlOptions.Value;
        }
        
        public async Task<OperationResult> SignIn(string login, string pass, bool isPersistent = false, bool isBackend = true, bool isFromApi = false) {
            var rootSignResult = await _signInIfRootAsync(login, pass, isPersistent, isBackend);
            if (rootSignResult != null) {
                return rootSignResult;
            }

            var user = await _uow.Users.GetByLoginAsync(login);

            while (true) {
                if (user != null && (await _IsNewPasswordValidAsync(user.Id, HashHelper.TransformPassword(pass)))) {
                    if (!isBackend) break;

                    if (isBackend && user.IsManager) break;
                }

                if (user == null || !user.IsPasswordValid(pass) || (!isFromApi && isBackend && !user.IsManager))
                {
                    return OperationResult.Failure("Неверный логин или пароль!");
                }

                break;
            }
            if (!isFromApi) {
                var userSession = UserSession.CreateSession(user.Id, user.Login, user.IsManager);

                if (isBackend) {
                    await SetClaimsAsync(userSession, isPersistent);
                } else {
                    await SetFrontClaimsAsync(userSession, isPersistent);
                }
            }

            try {
                await _logSignInAsync(user);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to log user sign in by login {login}. {message}", login, ex.Message);
            }

            return OperationResult.Success("Вы успешно авторизовались.");
        }
        
        public async Task<OperationResult> SignIn(AuthorizationLinkDTO link) {
            var user = await _uow.Users.GetByIdWithDataAsync(link.UserId);
            if (user == null) {
                return OperationResult.Failure("Не удалось авторизоваться. Ссылка не валидна!");
            }

            var session = UserSession.CreateLinkSession(user.Id, user.Login, new LinkSignIn(link.Type, link.Code));
            await SetFrontClaimsAsync(session, true);

            try {
                await _logSignInAsync(user);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to log user sign in {userId}. {message}", link.UserId, ex.Message);
            }

            return OperationResult.Success("Вы успешно авторизовались.");
        }
        
        public async Task<ClaimsPrincipal> SignIn(GuestAccessInfo guestAccessInfo) {
            ClaimsPrincipal principal = null;
            try {

                int oldGuestUserId = 0;

                try {
                    var frontGuest = await _httpContextAccessor.HttpContext.AuthenticateAsync(GlobalConstant.Session.FrontendGuestScheme);
                    if (frontGuest?.Principal != null) {
                        _httpContextAccessor.HttpContext.User = frontGuest?.Principal;
                    }
                    if (_httpContextAccessor.HttpContext.User.HasClaim("Type", GlobalConstant.Session.FrontendGuestScheme)) {
                        var claim = _httpContextAccessor.HttpContext.User.FindFirstValue(GlobalConstant.Session.FrontendGuestClaimName);
                        if (!string.IsNullOrEmpty(claim)) {
                            var curUser = JsonSerializer.Deserialize<GuestSession>(claim);
                            oldGuestUserId = curUser.UserId;
                        }
                    }
                } catch (Exception) {
                    oldGuestUserId = 0;
                }

                await SignOutGuest();
                await SignOut(false);

                GuestSession guestInfo = new GuestSession() {
                    Login = "Guest",
                    Id = Guid.NewGuid(),
                    GuestAccessIngo = guestAccessInfo
                };
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(GlobalConstant.Session.FrontendGuestClaimName, JsonSerializer.Serialize(guestInfo)));
                identity.AddClaim(new Claim("Type", GlobalConstant.Session.FrontendGuestScheme));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Guest"));
                var p = new ClaimsPrincipal(identity);
                var lifeTimeInDays = (guestAccessInfo.ExpiredDate - DateTime.Now).TotalDays;
                if (lifeTimeInDays < 1.0d) lifeTimeInDays = 1.0d;
                await _httpContextAccessor.HttpContext.SignInAsync(GlobalConstant.Session.FrontendGuestScheme, p,
                    new AuthenticationProperties {
                        ExpiresUtc = DateTime.UtcNow.AddDays(lifeTimeInDays),
                        IsPersistent = true,
                    });

                principal = p;

                if (oldGuestUserId < 0) {
                    await _context.Database.ExecuteSqlRawAsync($"UPDATE `cart_items_1c` SET `user_id`={guestInfo.UserId} WHERE `user_id`={oldGuestUserId};");
                }
            } catch (Exception) {
                principal = null;
            }

            return principal;
        }
        
        public async Task SignOut(bool isBackend = true) {
            if (isBackend)
                await _httpContextAccessor.HttpContext.SignOutAsync(GlobalConstant.Session.BackendScheme);
            else
                await _httpContextAccessor.HttpContext.SignOutAsync(GlobalConstant.Session.FrontendScheme);
        }
        
        public async Task SignOutGuest() {
            await _httpContextAccessor.HttpContext.SignOutAsync(GlobalConstant.Session.FrontendGuestScheme);
        }
        
        public string GetGuestLink(DateTime expiredDate, bool isPreoderVisible, bool isInstockByLinesVisible, bool isInstockByPartsVisible) {
            var paramsStr = _serializeGuestParams(expiredDate, isPreoderVisible, isInstockByLinesVisible, isInstockByPartsVisible);
            var checkHash = HashHelper.TransformPassword(paramsStr);
            var encodedStr = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{paramsStr}&hash={checkHash}"));

            return $"{_urlSettings.KristaShopUrl}?guestAccess={HttpUtility.UrlEncode(encodedStr)}";
        }
        
        public GuestAccessInfo DecodeGuestAccessStr(string value) {
            var result = new GuestAccessInfo();

            value = HttpUtility.UrlDecode(value);

            string decodedString = string.Empty;
            try {
                byte[] data = Convert.FromBase64String(value);
                decodedString = Encoding.UTF8.GetString(data);
            } catch (Exception) {
                return result;
            }
            if (string.IsNullOrEmpty(decodedString)) {
                return result;
            }

            var tmpDict = new Dictionary<string, string>();
            var tmpParts1 = decodedString.Split(new char[] { '&' });
            foreach (var part1 in tmpParts1) {
                var tmpParts2 = part1.Split(new char[] { '=' });
                if (tmpParts2.Length >= 2) {
                    if (!tmpDict.ContainsKey(tmpParts2[0].Trim())) {
                        tmpDict.Add(tmpParts2[0].Trim(), tmpParts2[1].Trim());
                    }
                }
            }

            if (tmpDict.ContainsKey("isInstockByLinesVisible")) result.IsInstockByLinesVisible = (tmpDict["isInstockByLinesVisible"].ToLower() == "true");
            if (tmpDict.ContainsKey("isInstockByPartsVisible")) result.IsInstockByPartsVisible = (tmpDict["isInstockByPartsVisible"].ToLower() == "true");
            if (tmpDict.ContainsKey("isPreoderVisible")) result.IsPreoderVisible = (tmpDict["isPreoderVisible"].ToLower() == "true");
            if (tmpDict.ContainsKey("expiredDate")) {
                try {
                    result.ExpiredDate = DateTime.ParseExact(tmpDict["expiredDate"], "yyyy-MM-ddTHH:mm:ss", null);
                } catch (Exception) {
                    result.ExpiredDate = DateTime.MinValue;
                }
            }
            if (tmpDict.ContainsKey("hash")) result.Hash = tmpDict["hash"];

            return result;
        }
        
        public bool IsGuestAccessInfoCorrect(string value) {
            return IsGuestAccessInfoCorrect(DecodeGuestAccessStr(value));
        }

        private async Task<OperationResult> _signInIfRootAsync(string login, string pass, bool isPersistent = false, bool isBackend = true) {
            if (isBackend && login.Equals(_settings.RootLogin)) {
                if (HashHelper.TransformPassword(pass).Equals(_settings.RootPassword)) {
                    await SetClaimsAsync(UserSession.CreateRootSession(), isPersistent);
                    return OperationResult.Success();
                }

                return OperationResult.Failure();
            }

            return null;
        }
        
        private async Task<bool> _IsNewPasswordValidAsync(int userId, string pass) {
            return await _uow.UserNewPasswords.All.Where(x => x.UserId == userId && x.Password == pass).AnyAsync();
        }

        private async Task _logSignInAsync(User user) {
            if (user.Data == null) {
                user.Data = new UserData(user.Id);
                await _uow.UserData.AddAsync(user.Data);
            } else {
                user.Data.LastSignIn = DateTimeOffset.Now;
                _uow.UserData.Update(user.Data);
            }
            await _uow.SaveChangesAsync();
        }
        
        private async Task SetClaimsAsync(IBaseSession user, bool isPersistent) {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(GlobalConstant.Session.BackendUserClaimName, JsonSerializer.Serialize(user)));
            identity.AddClaim(new Claim("Type", GlobalConstant.Session.BackendScheme));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Login));
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext!.AuthenticateAsync(GlobalConstant.Session.BackendScheme);
            await _httpContextAccessor.HttpContext.SignInAsync(GlobalConstant.Session.BackendScheme, principal,
                new AuthenticationProperties {
                    ExpiresUtc = isPersistent ? DateTime.UtcNow.AddDays(14) : (DateTime?) null,
                    IsPersistent = isPersistent
                });
        }

        private async Task SetFrontClaimsAsync(IBaseSession user, bool isPersistent) {
            await SignOutGuest();

            var claims = new List<Claim> {
                new(GlobalConstant.Session.FrontendUserClaimName, JsonSerializer.Serialize(user)),
                new("Type", GlobalConstant.Session.FrontendScheme),
                new(ClaimTypes.Name, user.Login)
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType));
            await _httpContextAccessor.HttpContext!.AuthenticateAsync(GlobalConstant.Session.FrontendScheme);
            await _httpContextAccessor.HttpContext.SignInAsync(GlobalConstant.Session.FrontendScheme, principal,
                new AuthenticationProperties {
                    ExpiresUtc = isPersistent ? DateTime.UtcNow.AddDays(14) : (DateTime?)null,
                    IsPersistent = isPersistent
                });
        }

        public bool IsGuestAccessInfoCorrect(GuestAccessInfo value) {
             var paramsStr = _serializeGuestParams(value.ExpiredDate, value.IsPreoderVisible, value.IsInstockByLinesVisible, value.IsInstockByPartsVisible);
             var checkHash = HashHelper.TransformPassword(paramsStr);

             if (string.IsNullOrEmpty(value.Hash) || checkHash != value.Hash) return false;

             if (value.ExpiredDate < DateTime.Now) return false;

             if (!value.IsInstockByLinesVisible && !value.IsInstockByPartsVisible && !value.IsPreoderVisible) return false;

             return true;
         }

        private string _serializeGuestParams(DateTime expiredDate, bool isPreoderVisible, bool isInstockByLinesVisible, bool isInstockByPartsVisible) {
             return $"expiredDate={expiredDate.ToString("yyyy-MM-ddTHH:mm:ss")}&isPreoderVisible={isPreoderVisible}&isInstockByLinesVisible={isInstockByLinesVisible}&isInstockByPartsVisible={isInstockByPartsVisible}";
         }
    }
}