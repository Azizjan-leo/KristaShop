using System.Threading.Tasks;

namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public interface IAsyncChainOperation<TOutput> : IChainOperation<TOutput> { }

    public interface IAsyncChainOperation<in TInput, TOutput> : IAsyncChainOperation<TOutput> {
        Task HandleAsync(TInput input);
    }
}