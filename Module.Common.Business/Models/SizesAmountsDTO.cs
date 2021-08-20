using System.Collections.Generic;

namespace Module.Common.Business.Models {
    public class SizesAmountsDTO {
        public List<string> Values { get; set; }
        public Dictionary<string, int> SizeColorAmount { get; set; }
        public Dictionary<string, int> TotalAmountBySize { get; set; }
    }
}