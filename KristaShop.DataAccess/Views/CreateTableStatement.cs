namespace KristaShop.DataAccess.Views {
    public class CreateTableStatement {
        public string Table { get; set; }
        public string CreateTable { get; set; }
    }

    public class Scalar {
        public string Value { get; set; }
    }

    public class ScalarInt {
        public int Value { get; set; }
    }

    public class ScalarULong {
        public ulong Value { get; set; }
    }
}
