using System;
using System.Threading.Tasks;

namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public class FuncChainAsyncOperation<TInput, TOutput> : IAsyncChainOperation<TInput, TOutput> {
        private readonly Func<TInput, Task<TOutput>> _function;
        public TOutput Result { get; set; }

        public FuncChainAsyncOperation(Func<TInput, Task<TOutput>> function) {
            _function = function;
        }

        public async Task HandleAsync(TInput input) {
            Result = await _function.Invoke(input);
        }
    }
}