using Microsoft.AspNetCore.Mvc;
using SettingsService.Models;
using SettingsService.Services;
using StackExchange.Redis;

namespace SettingsService.Data.Repositories.SettingsRepo
{

    public class SettingsRepo : ISettingsRepo
    {
        private readonly RedisSetupService _redisSetupService;
        private readonly IDatabase _redisDatabase;

        public SettingsRepo(RedisSetupService redisSetupService)
        {
            _redisSetupService = redisSetupService;
            _redisDatabase = _redisSetupService.InitializeRedisSettings("settingsdb");
        }

        public async Task<(string?, RepoAnswer)> GetSettingAsync(string key)
        {
            var value = await _redisDatabase.StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return (null, RepoAnswer.NotFound); 
            }
            return (value, RepoAnswer.Success); 
        }

        public async Task<(Setting?, RepoAnswer)> SetSettingAsync(string key, string value)
        {
            var settingCorrectlySet = await _redisDatabase.StringSetAsync(key, value);

            if (!settingCorrectlySet)
            {
                return (null, RepoAnswer.ActionFailed);
            }

            var setting = new Setting { Key = key, Value = value };
            return (setting, RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteSettingAsync(string key)
        {
            var settingCorrectlyDeleted = await _redisDatabase.KeyDeleteAsync(key);

            if (!settingCorrectlyDeleted)
            {
                return RepoAnswer.ActionFailed;
            }
            return RepoAnswer.Success;
        }
    }
}
