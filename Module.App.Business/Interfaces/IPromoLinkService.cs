using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using Module.App.Business.Models;

namespace Module.App.Business.Interfaces {
    public interface IPromoLinkService {
        Task<List<PromoLinkDTO>> GetPromoLinksAsync(UserSession userSession);
        Task<PromoLinkDTO> GetPromoLinkAsync(Guid promoLinkId);
        Task<OperationResult> InsertPromoLinkAsync(PromoLinkDTO promoLink);
        Task<OperationResult> UpdatePromoLinkAsync(PromoLinkDTO promoLink);
        Task<OperationResult> DeletePromoLinkAsync(Guid promoLinkId);
    }
}