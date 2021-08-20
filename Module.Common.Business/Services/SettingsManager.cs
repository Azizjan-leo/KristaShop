using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using KristaShop.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Module.Common.Business.Interfaces;
using Module.Common.Business.UnitOfWork;
using Serilog;

namespace Module.Common.Business.Services {
    public class SettingsManager<T> : ISettingsManager where T : class {
        public IAppSettings Settings { get; protected set; }
        protected IServiceScope ServiceScope;
        protected ConcurrentDictionary<string, string> SettingsDictionary;
        protected ILogger Logger;

        public SettingsManager() {
            SettingsDictionary = new ConcurrentDictionary<string, string>();
        }

        public virtual async Task InitializeAsync(IServiceScope serviceScope) {
            ServiceScope = serviceScope;
            Logger = serviceScope.ServiceProvider.GetService<ILogger>();

            _createAppSettingsObject();
            await ReloadAsync();
        }

        public async Task ReloadAsync() {
            try {
                var settingsList = await _getSettingsRepository().Settings.GetAllAsync();

                SettingsDictionary.Clear();
                foreach (var setting  in settingsList) {
                    if (SettingsDictionary.TryAdd(setting.Key, setting.Value)) {
                        _updateAppSettingsObject(setting.Key, setting.Value);
                    } else {
                        Logger.Error("Failed to add item to settings dictionary {@setting}", setting);
                    }
                }
            } catch (Exception ex) {
                Logger.Fatal(ex, "Failed to load settings dictionary. {message}", ex.Message);
            }
        }

        public async Task ReloadAsync(Guid settingId, string key = "") {
            try {
                var settings = await _getSettingsRepository().Settings.GetByIdAsync(settingId);
                if (settings == null) {
                    if (!string.IsNullOrEmpty(key)) {
                        _removeSetting(key);
                    }
                } else {
                    _updateSetting(settings);
                }
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to reload settings dictionary. {message}", ex.Message);
            }
        }

        public bool TryGetValue(string key, out string value) {
            value = "";
            try {
                if (SettingsDictionary.ContainsKey(key)) {
                    value = SettingsDictionary[key];
                    return true;
                }
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to get item from settings dictionary, item key: {key}. {message}", key, ex.Message);
                return false;
            }

            return false;
        }

        protected virtual void _removeSetting(string key) {
            if (SettingsDictionary.ContainsKey(key)) {
                if (SettingsDictionary.TryRemove(key, out var value)) {
                    _updateAppSettingsObject(key, string.Empty);
                } else {
                    Logger.Error("Failed to remove item from settings dictionary. Item key: {key}", key);
                }
            }
        }

        protected virtual void _updateSetting(Settings settings) {
            if (SettingsDictionary.ContainsKey(settings.Key)) {
                SettingsDictionary[settings.Key] = settings.Value;
                _updateAppSettingsObject(settings.Key, settings.Value);
            } else {
                if (SettingsDictionary.TryAdd(settings.Key, settings.Value)) {
                    _updateAppSettingsObject(settings.Key, settings.Value);
                } else {
                    Logger.Error("Failed to remove item from settings dictionary. Item: {@settings}", settings);
                }
            }
        }

        protected virtual void _createAppSettingsObject() {
            try {
                var settingsInstance = Activator.CreateInstance<T>();
                if (settingsInstance is IAppSettings settings) {
                    Settings = settings;
                } else {
                    throw new InvalidCastException($"{typeof(T).FullName} is not of type {typeof(IAppSettings).FullName}");
                }
            } catch (Exception ex) {
                 Logger.Fatal(ex, "Failed to create {appSettings} instance. {message}", typeof(IAppSettings).FullName, ex.Message);
            }
        }

        protected virtual void _updateAppSettingsObject(string key, string value) {
            if (Settings == null) return;

            var property = Settings.GetType().GetProperty(key);
            if (property != null) {
                object? convertedValue = null;
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (converter.CanConvertFrom(value.GetType())) {
                    convertedValue = converter.ConvertFrom(value);
                }

                if (convertedValue == null) {
                    convertedValue = Convert.ChangeType(value, property.PropertyType);
                }
                
                property.SetValue(Settings, convertedValue);
            }
        }

        private ICommonUnitOfWork _getSettingsRepository() {
            return ServiceScope.ServiceProvider.GetRequiredService<ICommonUnitOfWork>();
        }
    }
}
