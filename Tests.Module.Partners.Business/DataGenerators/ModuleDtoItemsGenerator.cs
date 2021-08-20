using System.Collections.Generic;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Tests.Common.DataGenerators;

namespace Tests.Module.Partners.Business.DataGenerators {
    public class ModuleDtoItemsGenerator : DtoItemsGenerator {
        public IEnumerable<DocumentItemDTO> CreateDocumentItemsDto(int count, string forArticul, int fromSize) {
            var result = new List<DocumentItemDTO>();
            for (var i = 0; i < count; i++) {
                result.Add(new DocumentItemDTO {Articul = forArticul, Size = new SizeValue((fromSize + 2 * 0).ToString())});
            }

            return result;
        }
        
        public IEnumerable<DocumentItemDTO> CreateDocumentItemsDto(int count, int modelId, string forArticul, int fromSize) {
            var result = new List<DocumentItemDTO>();
            for (var i = 0; i < count; i++) {
                result.Add(new DocumentItemDTO {ModelId = modelId, Articul = forArticul, Size = new SizeValue((fromSize + 2 * 0).ToString())});
            }

            return result;
        }
        
        public IEnumerable<DocumentItemDTO> CreateDocumentItemsDto(int count, string forArticul, int fromSize, int colorId) {
            var result = new List<DocumentItemDTO>();
            for (var i = 0; i < count; i++) {
                result.Add(new DocumentItemDTO {Articul = forArticul, Size = new SizeValue((fromSize + 2 * 0).ToString()), ColorId = colorId});
            }

            return result;
        }
    }
}