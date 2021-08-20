namespace KristaShop.Common.Models.Structs {
    public readonly struct RouteValue {
        public string Area { get; }
        public string Controller { get; }
        public string Action { get; }

        public RouteValue(string area, string controller, string action) : this() {
            Area = area;
            Controller = controller;
            Action = action;
        }
    }
}
