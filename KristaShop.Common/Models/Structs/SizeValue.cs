using System;

namespace KristaShop.Common.Models.Structs {
    public readonly struct SizeValue {
        public string Value { get; }
        public string Line { get; }
        public int Parts => IsLine ? Values.Length : 1;
        public bool IsLine => string.IsNullOrEmpty(Value) || Value.Contains("-");
        public string[] Values => Value.Split("-", StringSplitOptions.RemoveEmptyEntries);

        public SizeValue(string value) {
            if (string.IsNullOrEmpty(value)) {
                value = "Стандарт";
            }
            
            var sizes = value.Split("|", StringSplitOptions.RemoveEmptyEntries);
            Line = sizes.Length > 1 ? sizes[1] : sizes[0];
            Value = sizes[0];
        }

        public SizeValue(string value, string line) {
            value = value.Trim();
            if (string.IsNullOrEmpty(value)) {
                value = !string.IsNullOrEmpty(line) ? line : "Стандарт";
            }

            Line = line;
            Value = value;
        }

        public static bool operator ==(SizeValue x, SizeValue y) {
            return x.Equals(y);
        }

        public static bool operator !=(SizeValue x, SizeValue y) {
            return !x.Equals(y);
        }
        
        public override bool Equals(object obj) {
            return obj is SizeValue other && Equals(other);
        }

        public bool Equals(SizeValue other) {
            return Value.Equals(other.Value) && Line.Equals(other.Line);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Value, Line);
        }

        public override string ToString() {
            return $"{Value}|{Line}";
        }
    }
}