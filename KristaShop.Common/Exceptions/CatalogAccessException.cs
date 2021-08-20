using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Exceptions {
    public class CatalogAccessException : ExceptionBase {
        public CatalogAccessException(int userId, CatalogType type) 
            : base($"Access forbidden to the catalog {type} for user {userId}",
                $"Доступ к каталогу {type.GetDisplayName()} закрыт") { }
    }
}