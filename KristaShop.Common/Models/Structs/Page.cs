namespace KristaShop.Common.Models.Structs {
    public readonly struct Page {
        private const int DefaultSize = 20;
        public int Index { get; }
        public int Size { get; }
        
        public Page(int index, int size) {
            Index = index;
            Size = size;
        }

        public bool IsValid() {
            return Index >= 0 && Size > 0;
        }

        public int GetSizeToSkip() {
            return Index * Size;
        }
        
        /// <summary>
        /// Creates page safely, i.e if index or size are not valid,
        /// sets default valid values. Default size value = 20
        /// </summary>
        public static Page Create(int index, int size) {
            if (index < 0) {
                index = 0;
            }

            if (size <= 0) {
                size = DefaultSize;
            }

            return new Page(index, size);
        }
    }
}