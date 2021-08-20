using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.Common.Implementation.DataAccess {
    public abstract class EntityBase<TKey> : IEntityBase<TKey> {
        public abstract TKey GetId();
    }
}