using System.Threading.Tasks;

namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public interface IAsyncChain<TChainOutput> {
        Task<TChainOutput> ExecuteAsync();
    }
}