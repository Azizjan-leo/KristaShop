using Newtonsoft.Json;

namespace KristaShop.Common.Models.DTOs {
    public class ManagerDTO {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ManagerDetailsDTO Details { get; set; }
    }
}
