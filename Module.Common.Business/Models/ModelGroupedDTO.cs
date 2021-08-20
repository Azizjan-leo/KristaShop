using System.Collections.Generic;

namespace Module.Common.Business.Models {
    public class ModelGroupedDTO {
        public string ModelKey { get; set; }
        public ModelInfoDTO ModelInfo { get; set; }
        public List<ColorDTO> Colors { get; set; }
        public int TotalAmount { get; set; }
        public Dictionary<int, int> TotalAmountByColor { get; set; }
        public double TotalSum { get; set; }
        public Dictionary<int, double> TotalSumByColor { get; set; }
        public SizesAmountsDTO SizesInfo { get; set; }
        public List<BarcodeDTO> Barcodes { get; set; }

        public int GetUniqueModelsCount() {
            return SizesInfo.SizeColorAmount.Count;
        }
    }
}