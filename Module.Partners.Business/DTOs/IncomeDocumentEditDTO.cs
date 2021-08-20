using System;
using System.Collections.Generic;

namespace Module.Partners.Business.DTOs {
    public class IncomeDocumentEditDTO : DocumentEditDTO {
        public DateTime Date { get; set; }
        public IEnumerable<DocumentItemDTO> ExcessItems { get; set; }
        public IEnumerable<DocumentItemDTO> DeficiencyItems { get; set; }
    }
}