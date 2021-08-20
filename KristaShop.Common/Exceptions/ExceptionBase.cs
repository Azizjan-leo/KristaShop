using System;

namespace KristaShop.Common.Exceptions {
    public class ExceptionBase : Exception {
        public bool IsSystem { get; protected set; }
        public string ReadableMessage { get; protected set; }

        public ExceptionBase() { }

        public ExceptionBase(Exception ex) : base(ex.Message, ex) {
            IsSystem = true;
        }

        public ExceptionBase(Exception innerException, string readableMessage)
            : base(innerException.Message, innerException) {
            IsSystem = string.IsNullOrEmpty(readableMessage);
            ReadableMessage = readableMessage;
        }

        public ExceptionBase(string systemMessage, Exception innerException = null) 
            : base(systemMessage, innerException) {
            IsSystem = true;
            ReadableMessage = string.Empty;
        }

        public ExceptionBase(string systemMessage, string readableMessage, Exception innerException = null)
            : base(systemMessage, innerException) {
            IsSystem = string.IsNullOrEmpty(readableMessage);
            ReadableMessage = readableMessage;
        }

        public override string ToString() {
            return $"{nameof(IsSystem)}: {IsSystem}, {nameof(ReadableMessage)}: {ReadableMessage}\r\n{base.ToString()}";
        }
    }
}
