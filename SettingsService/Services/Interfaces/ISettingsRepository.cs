﻿using SettingsService.Data.Models;

namespace SettingsService.Services.Interfaces
{
    public interface ISettingsRepository
    {
        public Task<(List<Setting>, RepoAnswer)> GetAllSettingsAsync();
        public Task<(string?, RepoAnswer)> GetSettingAsync(string key);
        public Task<(Setting?, RepoAnswer)> SetSettingAsync(string key, string value);
        public Task<RepoAnswer> DeleteSettingAsync(string key);
    }
}
