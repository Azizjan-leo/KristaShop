namespace Module.Catalogs.Business.Models {
    public class ModelPhotoDTO {
        public string Articul { get; set; }
        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public string OldPhotoPath { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public int Order { get; set; }
    }
}