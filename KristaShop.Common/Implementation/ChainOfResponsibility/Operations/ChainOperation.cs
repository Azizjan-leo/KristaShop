using System.Threading.Tasks;

namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public abstract class ChainOperation<TInput, TOutput> : IAsyncChainOperation<TInput, TOutput>, IChainOperation<TInput, TOutput> {
        public TOutput Result { get; set; }

        public async Task HandleAsync(TInput input) {
            Result = await HandleInputAsync(input);
        }
        
        public void Handle(TInput input) {
            Result = HandleInput(input);
        }

        protected abstract TOutput HandleInput(TInput input);
        protected abstract Task<TOutput> HandleInputAsync(TInput input);
    }
}