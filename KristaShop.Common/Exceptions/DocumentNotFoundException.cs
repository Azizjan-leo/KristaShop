using System;
using System.Reflection;

namespace KristaShop.Common.Exceptions {
    public class DocumentNotFoundException : EntityNotFoundException {
        public DocumentNotFoundException(ulong number, int userId) 
            : base($"Document {number} not exists for user {userId}", $"Документ {number} не найден") { }
        
        public DocumentNotFoundException(Guid id) 
            : base($"Document {id} not exists", $"Документ {id} не найден") { }
        
        public DocumentNotFoundException(Guid id, int userId) 
            : base($"Document {id} not exists for user {userId}", $"Документ {id} не найден") { }
        
        public DocumentNotFoundException(MemberInfo type) 
            : base($"Document of type (${type.Name}) not exists", $"Документ не найден") { }
    }
}