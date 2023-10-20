using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using SettingsService.Configuration;
using SettingsService.Data.Models;
using SettingsService.Dto;
using SettingsService.Services.Interfaces;
using StackExchange.Redis;

namespace SettingsService.Services.Repository
{

    public class SettingsRepository : ISettingsRepository
    {
        private readonly IDatabase _redisDatabase;
        private readonly IValidator<Setting> _settingValidator;
        private readonly IDataProtectionProvider _provider;

        public SettingsRepository(RedisSetup redisSetupService, IValidator<Setting> settingValidator, IDataProtectionProvider provider)
        {
            _redisDatabase = redisSetupService.InitializeRedisSettings("settingsdb");

            _settingValidator = settingValidator;
            _provider = provider;
        }

        public async Task<(List<Setting>, RepoAnswer)> GetAllSettingsAsync()
        {
            var settings = new List<Setting>();
            var keys = await _redisDatabase.ExecuteAsync("KEYS", "*"); // Получаем все ключи

            foreach (var key in (string[])keys!)
            {
                var value = await _redisDatabase.StringGetAsync(key);
                settings.Add(new Setting { Key = key, Value = value! });
            }

            return settings.Count > 0 ? (settings, RepoAnswer.Success) : (settings, RepoAnswer.NotFound);
        }

        public async Task<(string?, RepoAnswer)> GetSettingAsync(GetSettingDto getSetting)
        {
            getSetting.Key = getSetting.Key.Trim();
            var value = await _redisDatabase.StringGetAsync(getSetting.Key);
            if (string.IsNullOrEmpty(value))
            {
                return (null, RepoAnswer.NotFound);
            }
            // Расшифруем значение
            var decryptedValue = Decrypt(value!, getSetting.EncryptionKey);
            return (decryptedValue, RepoAnswer.Success);
        }

        public async Task<(Setting?, RepoAnswer)> SetSettingAsync(SetSettingDto setSetting)
        {
            setSetting.Key = setSetting.Key.Trim();
            setSetting.Value = setSetting.Value.Trim();
            var setting = new Setting { Key = setSetting.Key, Value = setSetting.Value };
            var validationResult = await _settingValidator.ValidateAsync(setting);
            if (!validationResult.IsValid)
            {
                return (null, RepoAnswer.ActionFailed); // Валидация не прошла
            }

            // Шифруем значение
            var encryptedValue = Encrypt(setSetting.Value, setSetting.EncryptionKey);
            var settingCorrectlySet = await _redisDatabase.StringSetAsync(setting.Key, encryptedValue);

            return !settingCorrectlySet ? (null, RepoAnswer.ActionFailed) : (setting, RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteSettingAsync(string key)
        {
            key = key.Trim();
            var settingCorrectlyDeleted = await _redisDatabase.KeyDeleteAsync(key);

            return !settingCorrectlyDeleted ? RepoAnswer.ActionFailed : RepoAnswer.Success;
        }

        public string Encrypt(string text, string sercretKey)
        {
            var protector = _provider.CreateProtector(sercretKey);
            return protector.Protect(text);
        }

        public string Decrypt(string text, string sercretKey)
        {
            var protector = _provider.CreateProtector(sercretKey);
            return protector.Unprotect(text);
        }
    }
}
