using System;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Common.Business.Interfaces {
    public interface ISettingsManager {
        IAppSettings Settings { get; }
        Task InitializeAsync(IServiceScope serviceScope);
        Task ReloadAsync();
        Task ReloadAsync(Guid settingId, string key = "");
        bool TryGetValue(string key, out string value);
    }
}
