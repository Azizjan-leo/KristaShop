using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Views.Partners;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IPartnersRepository : IRepository<Partner, int> {
        Task<List<PartnerSalesReportItem>> GetSalesReportItems(ReportsFilter filter);
        Task<List<DocumentItem>> GetDecryptedSalesReportItems(ReportsFilter filter);
        Task<List<PartnerSqlView>> GetAllPartnersAsync(PartnersFilter filter);
        Task<List<LookUpItem<int, string>>> GetPartnersLookUpAsync();
        Task<double> GetPartnerPaymentRateAsync(int userId);
        Task<bool> IsPartnerAsync(int userId);
    }
}
