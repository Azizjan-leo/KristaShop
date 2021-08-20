using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Module.Order.Business.Interfaces;
using Quartz;

namespace Module.Cart.WebUI.Jobs {
    [DisallowConcurrentExecution]
    public class CleanOldCartsJob : IJob {
        private readonly IServiceProvider _provider;
        
        public CleanOldCartsJob(IServiceProvider provider) {
            _provider = provider;
        }
        
        public async Task Execute(IJobExecutionContext context) {
            using (var scope = _provider.CreateScope()) {
                var cartService = scope.ServiceProvider.GetService<ICartService>();
                await cartService.ClearOldItemsAsync();
            }
        }
    }
}