namespace Module.Common.WebUI.Models {
    public class ComponentOutValue<T> {
        public T Value { get; set; }

        public ComponentOutValue(T value) {
            Value = value;
        }
    }
}