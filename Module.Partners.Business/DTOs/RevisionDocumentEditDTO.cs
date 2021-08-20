using System.Collections.Generic;

namespace Module.Partners.Business.DTOs {
    public class RevisionDocumentEditDTO : DocumentEditDTO {
        public IEnumerable<DocumentItemDTO> ExcessItems { get; set; }
        public IEnumerable<DocumentItemDTO> DeficiencyItems { get; set; }

        public RevisionDocumentEditDTO() {
            ExcessItems = new List<DocumentItemDTO>();
            DeficiencyItems = new List<DocumentItemDTO>();
        }
    }
}