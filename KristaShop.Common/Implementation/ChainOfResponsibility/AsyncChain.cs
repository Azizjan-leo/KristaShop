using System;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;

namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public class AsyncChain<TChainInput, TChainOutput> : IAsyncChain<TChainOutput> {
        private readonly TChainInput _input;
        private readonly Func<Task<IAsyncChainOperation<TChainInput>>, Task<IAsyncChainOperation<TChainOutput>>> _chainFunction;

        public AsyncChain(TChainInput input, Func<Task<IAsyncChainOperation<TChainInput>>, Task<IAsyncChainOperation<TChainOutput>>> chainFunction) {
            _input = input;
            _chainFunction = chainFunction;
        }
        
        public async Task<TChainOutput> ExecuteAsync() {
            return await _chainFunction.Invoke(_createFirstChainItemAsync()).GetResultAsync();
        }
        
        private async Task<IAsyncChainOperation<TChainInput>> _createFirstChainItemAsync() {
            var operation = new FuncChainAsyncOperation<TChainInput, TChainInput>(async x => x);
            await operation.HandleAsync(_input);
            return operation;
        }
    }
}