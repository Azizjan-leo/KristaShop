using System;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;

namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public class SyncChain<TChainInput, TChainOutput> : ISyncChain<TChainOutput> {
        private readonly TChainInput _input;
        private readonly Func<IChainOperation<TChainInput>, IChainOperation<TChainOutput>> _chainFunction;

        public SyncChain(TChainInput input, Func<IChainOperation<TChainInput>, IChainOperation<TChainOutput>> chainFunction) {
            _input = input;
            _chainFunction = chainFunction;
        }
        
        public TChainOutput Execute() {
            return _chainFunction.Invoke(_createFirstChainItemAsync()).Result;
        }
        
        private IChainOperation<TChainInput> _createFirstChainItemAsync() {
            var operation = new FuncChainSyncOperation<TChainInput, TChainInput>(x => x);
            operation.Handle(_input);
            return operation;
        }
    }
}