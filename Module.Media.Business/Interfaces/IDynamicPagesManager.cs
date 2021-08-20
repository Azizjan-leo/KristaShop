using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Module.Media.Business.DTOs;

namespace Module.Media.Business.Interfaces {
    public interface IDynamicPagesManager {
        Task InitializeAsync(IServiceScope serviceScope);
        Task ReloadAsync();
        Task ReloadAsync(Guid menuId, string oldKey = "");
        bool TryGetValue(string url, out DynamicPageDTO value);
        bool TryGetValuesByController(string controller, bool openOnly, out List<DynamicPageDTO> values);
        bool TryGetValuesByControllerForMenu(string controller, bool openOnly, out List<DynamicPageDTO> values);
    }
}
