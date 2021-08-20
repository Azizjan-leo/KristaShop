using System.Threading.Tasks;

namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public abstract class ChainAsyncOperation<TInput, TOutput> : IAsyncChainOperation<TInput, TOutput> {
        public TOutput Result { get; set; }

        public async Task HandleAsync(TInput input) {
            Result = await HandleInputAsync(input);
        }

        protected abstract Task<TOutput> HandleInputAsync(TInput input);
    }
}