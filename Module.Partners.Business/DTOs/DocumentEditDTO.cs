using System.Collections.Generic;

namespace Module.Partners.Business.DTOs {
    public class DocumentEditDTO {
        public int UserId { get; set; }
        public IEnumerable<DocumentItemDTO> Items { get; set; }

        public DocumentEditDTO() {
            Items = new List<DocumentItemDTO>();
        }

        public DocumentEditDTO(int userId, IEnumerable<DocumentItemDTO> items) {
            UserId = userId;
            Items = items;
        }

        public DocumentEditDTO(int userId, DocumentItemDTO item) {
            UserId = userId;
            Items = new List<DocumentItemDTO> {item};
        }
    }
}