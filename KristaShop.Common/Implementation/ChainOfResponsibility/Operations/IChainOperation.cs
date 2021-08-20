namespace KristaShop.Common.Implementation.ChainOfResponsibility.Operations {
    public interface IChainOperation<TOutput> {
        public TOutput Result { get; set; }
    }
    
    public interface IChainOperation<in TInput, TOutput> : IChainOperation<TOutput> {
        public void Handle(TInput input);
    }
}