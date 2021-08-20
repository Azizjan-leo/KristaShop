using System;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;

namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public static class ChainBuilder {
        public static ISyncChain<TLastOutput> Create<TInput, TLastOutput>(TInput input,
            Func<IChainOperation<TInput>, IChainOperation<TLastOutput>> builder) {
            return new SyncChain<TInput, TLastOutput>(input, builder);
        }
        
        public static async Task<IAsyncChain<TLastOutput>> CreateAsync<TInput, TLastOutput>(TInput input,
            Func<Task<IAsyncChainOperation<TInput>>, Task<IAsyncChainOperation<TLastOutput>>> builder) {
            return new AsyncChain<TInput, TLastOutput>(input, builder);
        }
    }
}