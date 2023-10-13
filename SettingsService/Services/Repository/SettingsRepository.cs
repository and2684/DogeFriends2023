using FluentValidation;
using SettingsService.Models;
using SettingsService.Services.Interfaces;
using StackExchange.Redis;

namespace SettingsService.Services.Repository
{

    public class SettingsRepository : ISettingsRepository
    {
        private readonly RedisSetupService _redisSetupService;
        private readonly IDatabase _redisDatabase;
        private readonly IValidator<Setting> _settingValidator;

        public SettingsRepository(RedisSetupService redisSetupService, IValidator<Setting> settingValidator)
        {
            _redisSetupService = redisSetupService;
            _redisDatabase = _redisSetupService.InitializeRedisSettings("settingsdb");

            _settingValidator = settingValidator;
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
            var validationResult = _settingValidator.Validate(setting);
            if (!validationResult.IsValid)
            {
                return (null, RepoAnswer.ActionFailed); // Валидация не прошла
            }

            var settingCorrectlySet = await _redisDatabase.StringSetAsync(setting.Key, setting.Value);
            if (!settingCorrectlySet)
            {
                return (null, RepoAnswer.ActionFailed);
            }

            return (setting, RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteSettingAsync(string key)
        {
            key = key.Trim();
            var settingCorrectlyDeleted = await _redisDatabase.KeyDeleteAsync(key);

            if (!settingCorrectlyDeleted)
            {
                return RepoAnswer.ActionFailed;
            }
            return RepoAnswer.Success;
        }
    }
}
