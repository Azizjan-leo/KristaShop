namespace KristaShop.DataAccess.Entities.Interfaces {
    public interface IUserNomenclature : INomenclature {
        int UserId { get; set; }
        User User { get; set; }
    }
}