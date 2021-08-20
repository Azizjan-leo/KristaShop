using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Session;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;

namespace Module.Catalogs.Business.Common {
    public static class UserSessionExtensions {
        public static async Task<List<Catalog1CDTO>> GetAvailableCatalogsAsync(this UserSession session, IUnitOfWork uow) {
            if (session == null || session is UnauthorizedSession) {
                return new List<Catalog1CDTO> {Catalog1CDTO.GetOpenCatalog()};
            }

            if (session.IsManager || session.IsRoot) {
                return CatalogTypeExtensions.GetAllCatalogs().Select(x => new Catalog1CDTO(x)).ToList();
            }

            if (!session.IsGuest()) {
                var catalogs = await uow.Users.GetUserAvailableCatalogsOrOpenCatalogAsync(session.UserId);
                return catalogs.Select(x => new Catalog1CDTO(x)).ToList();
            }
            
            var guestUser = session as GuestSession;
            return guestUser.GuestAccessIngo.GetOnlyAvailableCatalogsOrOpenCatalog()
                .Select(x => new Catalog1CDTO(x))
                .ToList();
        }
    }
}