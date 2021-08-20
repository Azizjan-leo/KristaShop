namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public interface ISyncChain<TChainOutput> {
        TChainOutput Execute();
    }
}