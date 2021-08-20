namespace KristaShop.Common.Exceptions {
    public class DocumentItemsException : ExceptionBase {
        public DocumentItemsException(string documentType)
            : base($"Document {documentType} should have items", $"Документ должен содержать модели") { }
    }
}