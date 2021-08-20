using System;

namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public class FuncChainSyncOperation<TInput, TOutput> : IChainOperation<TInput, TOutput> {
        private readonly Func<TInput, TOutput> _function;
        public TOutput Result { get; set; }

        public FuncChainSyncOperation(Func<TInput, TOutput> function) {
            _function = function;
        }

        public void Handle(TInput input) {
            Result = _function.Invoke(input);
        }
    }
}