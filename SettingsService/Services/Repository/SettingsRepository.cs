using FluentValidation;
using SettingsService.Data;
using SettingsService.Data.Models;
using SettingsService.Services.Interfaces;
using StackExchange.Redis;

namespace SettingsService.Services.Repository
{

    public class SettingsRepository : ISettingsRepository
    {
        private readonly IDatabase _redisDatabase;
        private readonly IValidator<Setting> _settingValidator;

        public SettingsRepository(RedisSetup redisSetupService, IValidator<Setting> settingValidator)
        {
            _redisDatabase = redisSetupService.InitializeRedisSettings("settingsdb");

            _settingValidator = settingValidator;
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

        public async Task<(string?, RepoAnswer)> GetSettingAsync(string key)
        {
            key = key.Trim();
            var value = await _redisDatabase.StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return (null, RepoAnswer.NotFound);
            }
            return (value, RepoAnswer.Success);
        }

        public async Task<(Setting?, RepoAnswer)> SetSettingAsync(string key, string value)
        {
            key = key.Trim();
            value = value.Trim();
            var setting = new Setting { Key = key, Value = value };
            var validationResult = await _settingValidator.ValidateAsync(setting);
            if (!validationResult.IsValid)
            {
                return (null, RepoAnswer.ActionFailed); // Валидация не прошла
            }

            var settingCorrectlySet = await _redisDatabase.StringSetAsync(setting.Key, setting.Value);
            return !settingCorrectlySet ? (null, RepoAnswer.ActionFailed) : (setting, RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteSettingAsync(string key)
        {
            key = key.Trim();
            var settingCorrectlyDeleted = await _redisDatabase.KeyDeleteAsync(key);

            return !settingCorrectlyDeleted ? RepoAnswer.ActionFailed : RepoAnswer.Success;
        }
    }
}
