using System.Collections.Generic;
using Module.Common.Business.Models;

namespace Module.Partners.Business.DTOs {
    public class StorehouseMovementGroupDTO<TItems> {
        public string ModelKey { get; set; }
        public ModelInfoDTO ModelInfo { get; set; }
        public List<TItems> Items { get; set; }
        public List<DocumentMovementItemDTO> Documents { get; set; }
    }
}