using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Filters;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;

namespace Module.Partners.Business.Interfaces {
    public interface IPartnerService {
        Task<OperationResult> ApplyAsync(int userId);
        Task<List<PartnershipRequestDTO>> GetNewRequests();
        Task<OperationResult> ApproveRequestAsync(Guid id);
        Task<OperationResult> DeleteRequestAsync(Guid id);
        Task<OperationResult> AcceptRequestToProcessAsync(Guid id);
        Task<ItemsGroupedBase<PartnerDTO, PartnersTotalsDTO>> GetAllPartnersAsync(PartnersFilter filter);
        Task<List<LookUpItem<int, string>>> GetPartnersLookUpAsync();
        Task<ItemsGrouped<PartnerSalesReportItem>> GetSalesReportAsync(ReportsFilter filter);
        Task<List<SimpleGroupedModelDTO<DocumentItemDetailedDTO>>> GetDecryptedSalesReportAsync(ReportsFilter filter);
        Task MakePartnerAsync(int userId);
    }
}