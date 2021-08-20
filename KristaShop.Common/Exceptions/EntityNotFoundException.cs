using System;

namespace KristaShop.Common.Exceptions {
    public class EntityNotFoundException : ExceptionBase {
        public EntityNotFoundException() {
        }

        public EntityNotFoundException(Exception ex) : base(ex) {
        }

        public EntityNotFoundException(Exception innerException, string readableMessage) : base(innerException, readableMessage) {
        }

        public EntityNotFoundException(string systemMessage, Exception innerException = null) : base(systemMessage, innerException) {
        }

        public EntityNotFoundException(string systemMessage, string readableMessage, Exception innerException = null) : base(systemMessage, readableMessage, innerException) {
        }
    }
}
