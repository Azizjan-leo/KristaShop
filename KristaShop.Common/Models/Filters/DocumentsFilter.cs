using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Models.Filters {
    public class DocumentsFilter : ModelsFilter {
        public SelectList DocumentTypesSelect { get; set; }
        public List<string> DocumentTypes { get; set; }

        public DocumentsFilter() {
            DocumentTypes = new List<string>();
        }
    }
}