namespace Module.Common.Business.Models {
    public class ColorDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }

        protected bool Equals(ColorDTO other) {
            return Id == other.Id;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ColorDTO) obj);
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}