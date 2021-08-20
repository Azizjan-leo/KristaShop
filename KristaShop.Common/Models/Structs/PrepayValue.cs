namespace KristaShop.Common.Models.Structs {
    public readonly struct PrepayValue {
        public string Name { get; }
        public double Percent { get; }

        public PrepayValue(string name, double percent) {
            Name = name;
            Percent = percent;
        }
    }
}