using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models.Filters;
using Module.Partners.Business.DTOs;

namespace Module.Partners.Business.Interfaces {
    public interface IPartnerStorehouseReportService {
        Task<PartnerTotalsDTO> GetTotalsAsync(int userId);
        Task<List<StorehouseMovementGroupDTO<StorehouseMovementItemDTO>>> GetModelsMovementAsync(int userId, ModelsFilter filter);
        Task<List<StorehouseMovementGroupDTO<StorehouseMovementItemDTO>>> GetModelMovementAsync(int userId, int modelId, DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}