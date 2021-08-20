namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public abstract class ChainSyncOperation<TInput, TOutput> : IChainOperation<TInput, TOutput> {
        public TOutput Result { get; set; }

        public void Handle(TInput input) {
            Result = HandleInput(input);
        }

        protected abstract TOutput HandleInput(TInput input);
    }
}