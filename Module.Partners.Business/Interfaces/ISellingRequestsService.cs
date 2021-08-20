using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Module.Partners.Business.Interfaces {
    public interface ISellingRequestsService {
        Task<List<SellingRequestDocumentDTO<ModelGroupedDTO>>> GetRequestsAsync(int userId);
        Task CreateSellingRequestAsync(int userId, IEnumerable<BarcodeAmountDTO> items);
        Task UpdateSellingRequestAsync(int userId, IEnumerable<BarcodeAmountDTO> items);
        Task UpdateSellingRequestStatusAsync(Guid documentId);
    }
}