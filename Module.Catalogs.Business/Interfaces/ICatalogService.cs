using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Business.Interfaces {
    public interface ICatalogService {
        Task<OperationResult> DeleteCatalog(CatalogType id, string filesDirectoryPath);
        Task<CatalogDTO> GetCatalogDetailsAsync(CatalogType catalogId);
        Task<CatalogDTO> GetCatalogDetailsWithExtraChargesAsync(CatalogType catalogId);
        Task<CatalogDTO> GetCatalogByUriOrOpenCatalogAsync(string uri);
        Task<List<CatalogDTO>> GetCatalogsAsync(bool includeDiscounts = false);
        Task<List<CatalogDTO>> GetCatalogsAsync(List<CatalogType> catalogIds);
        Task<OperationResult> InsertCatalog(CatalogDTO dto);
        Task<OperationResult> UpdateCatalog(CatalogDTO dto);
        List<Catalog1CDTO> GetAllCatalogs();
        Task UpdateCatalogPosition(CatalogType id, int position);
        Task AddOrSetExtraChargeToCatalog(CatalogType id, ExtraChargeType extraChargeType, double sum);
        Task<List<CatalogExtraChargeDTO>> GetCatalogExtraCharges(CatalogType id);
        Task DeleteCatalogExtraCharge(Guid id);
    }
}