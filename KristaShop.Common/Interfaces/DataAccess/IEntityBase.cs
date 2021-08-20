namespace KristaShop.Common.Interfaces.DataAccess {
    public interface IEntityBase<TKey> {
        TKey GetId();
    }
}