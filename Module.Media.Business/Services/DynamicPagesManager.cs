using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;
using Serilog;

namespace Module.Media.Business.Services {
    public class DynamicPagesManager : IDynamicPagesManager {
        protected IServiceScope ServiceScope { get; set; }
        protected ILogger Logger { get; set; }
        protected IMapper _mapper { get; set; }
        protected ConcurrentDictionary<string, DynamicPageDTO> Pages;

        public DynamicPagesManager() {
            Pages = new ConcurrentDictionary<string, DynamicPageDTO>();
        }

        public async Task InitializeAsync(IServiceScope serviceScope) {
            ServiceScope = serviceScope;
            Logger = serviceScope.ServiceProvider.GetService<ILogger>();
            _mapper = serviceScope.ServiceProvider.GetService<IMapper>();

            await ReloadAsync();
        }

        public async Task ReloadAsync() {
            try {
                var uow = ServiceScope.ServiceProvider.GetService<IUnitOfWork>();
                var contents = _mapper.Map<List<DynamicPageDTO>>(await uow.DynamicPage.All.AsNoTracking().ToListAsync());
                Pages.Clear();
                foreach (var content in contents) {
                    Pages.TryAdd(content.URL.ToLower(), content);
                }
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to reload dynamic pages manager. {message}", ex.Message);
            }
        }

        public async Task ReloadAsync(Guid menuId, string oldKey = "") {
            try {
                var uow = ServiceScope.ServiceProvider.GetService<IUnitOfWork>();
                var page = _mapper.Map<DynamicPageDTO>(await uow.DynamicPage.All.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == menuId));

                if (!string.IsNullOrEmpty(oldKey)) {
                    _removePage(oldKey);
                }

                if (page != null) {
                    _updatePages(page);
                }
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to reload dynamic pages manager. {message}", ex.Message);
            }
        }

        public bool TryGetValue(string url, out DynamicPageDTO value) {
            value = null;
            try {
                var key = url.ToLower();
                if (Pages.ContainsKey(key)) {
                    value = Pages[key];
                    return true;
                }
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to get item from dynamic pages manager, item key: {key}. {message}", url, ex.Message);
                return false;
            }

            return false;
        }

        public bool TryGetValuesByController(string controller, bool openOnly, out List<DynamicPageDTO> values) {
            values = null;
            try {
                values = openOnly
                    ? Pages.Values.Where(x => x.URL.Contains($"{controller}/") && x.IsOpen).OrderBy(x => x.Order).ToList()
                    : Pages.Values.Where(x => x.URL.Contains($"{controller}/")).OrderBy(x => x.Order).ToList();
                return true;
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to get items from dynamic pages manager by controller: {controller}. {message}", controller, ex.Message);
                return false;

            }
        }

        public bool TryGetValuesByControllerForMenu(string controller, bool openOnly, out List<DynamicPageDTO> values) {
            values = null;
            try {
                values = openOnly
                    ? Pages.Values.Where(x => x.URL.Contains($"{controller}/") && x.IsOpen && x.IsVisibleInMenu).OrderBy(x => x.Order).ToList()
                    : Pages.Values.Where(x => x.URL.Contains($"{controller}/") && x.IsVisibleInMenu).OrderBy(x => x.Order).ToList();
                return true;
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to get items from dynamic pages manager by controller: {controller}. {message}", controller, ex.Message);
                return false;

            }
        }

        protected virtual void _removePage(string key) {
            if (Pages.ContainsKey(key)) {
                if (!Pages.TryRemove(key, out _)) {
                    Logger.Error("Failed to remove item from dynamic pages manager. Item key: {key}", key);
                }
            }
        }

        protected virtual void _updatePages(DynamicPageDTO dynamicPage) {
            var key = dynamicPage.URL.ToLower();
            if (Pages.ContainsKey(key)) {
                Pages[key] = dynamicPage;
            } else {
                if (!Pages.TryAdd(key, dynamicPage)) {
                    Logger.Error("Failed to remove item from dynamic pages manager. Item: {@dynamicPage}", dynamicPage);
                }
            }
        }
    }
}
