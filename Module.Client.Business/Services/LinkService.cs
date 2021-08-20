using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.Client.Business.Interfaces;
using Module.Client.Business.Models;
using Module.Client.Business.UnitOfWork;
using Result = KristaShop.Common.Models.Result;

namespace Module.Client.Business.Services {
    public class LinkService : ILinkService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UrlSetting _urlSettings;

        public LinkService(IUnitOfWork uow, IMapper mapper, IOptions<UrlSetting> urlOptions) {
            _uow = uow;
            _mapper = mapper;
            _urlSettings = urlOptions.Value;
        }

        public async Task<IResult<AuthorizationLinkDTO>> GetUserIdByRandCodeAsync(string code) {
            var link = await _uow.AuthorizationLinks.All.FirstOrDefaultAsync(x => x.Code == code);
            if (link == null || !link.IsValid()) {
                return Result.ErrorModel<AuthorizationLinkDTO>(null, "Invalid signin link or code", "Ссылка на вход не валидна.");
            }

            if (link.Type == AuthorizationLinkType.SingleAccess) {
                _uow.AuthorizationLinks.Delete(link);
            } else {
                link.UpdateLoginDate();
                _uow.AuthorizationLinks.Update(link);
            }

            if (!await _uow.SaveAsync()) {
                return Result.ErrorModel<AuthorizationLinkDTO>(null, "Failed to save updated signin link", "Произошла ошибка при получении доступа по ссылке.");
            }

            return Result.SuccessModel(_mapper.Map<AuthorizationLinkDTO>(link));
        }

        public async Task<IResult<string>> InsertLinkAuthAsync(int userId, AuthorizationLinkType type = AuthorizationLinkType.MultipleAccess, bool fullPath = true) {
            var link = await _uow.AuthorizationLinks.All.FirstOrDefaultAsync(x => x.UserId == userId && x.Type == type);
            var path = fullPath ? $"{_urlSettings.KristaShopUrl}?" : string.Empty;
            if (link == null) {
                link = new AuthorizationLink(userId, type);
                await _uow.AuthorizationLinks.AddAsync(link, true);
            } else {
                link.UpdateActiveDays();
                _uow.AuthorizationLinks.Update(link);
            }

            if (!await _uow.SaveAsync()) {
                return Result.ErrorModel(string.Empty, "Failed to save inserted signin link", "Произошла ошибка при генерации ссылки доступа");
            }

            return Result.SuccessModel($"{path}randh={link.Code}");
        }

        public async Task<IResult> RemoveLinksByUserIdAsync(int userId) {
            var links = await _uow.AuthorizationLinks.All.Where(x => x.UserId == userId).ToListAsync();
            _uow.AuthorizationLinks.DeleteRange(links);
            if (!await _uow.SaveAsync()) {
                return Result.Error("Failed to delete signin links", "Не удалось удалить ссылки пользователя");
            }

            return Result.Success();
        }

        public async Task<IResult> RemoveLinkByCodeAsync(string code) {
            var link = await _uow.AuthorizationLinks.All.FirstOrDefaultAsync(x => x.Code == code);
            if (link != null) {
                _uow.AuthorizationLinks.Delete(link);
                if (!await _uow.SaveAsync()) {
                    return Result.Error("Failed to delete signin links", "Не удалось удалить ссылку пользователя");
                }
            }

            return Result.Success();
        }

        public async Task<bool> IsLinkExistAsync(string code) {
            var link = await _uow.AuthorizationLinks.All.FirstOrDefaultAsync(x => x.Code == code);
            return link != null;
        }
    }
}