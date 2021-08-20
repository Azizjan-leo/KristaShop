namespace Module.Common.Business.Models.Adapters {
    public interface IEntityToDtoAdapter<in TSource, out TDest> {
        TDest Convert(TSource source);
    }
}