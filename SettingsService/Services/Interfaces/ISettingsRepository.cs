using SettingsService.Data.Models;
using SettingsService.Dto;

namespace SettingsService.Services.Interfaces
{
    public interface ISettingsRepository
    {
        public Task<(List<Setting>, RepoAnswer)> GetAllSettingsAsync();
        public Task<(string?, RepoAnswer)> GetSettingAsync(GetSettingDto getSetting);
        public Task<(Setting?, RepoAnswer)> SetSettingAsync(SetSettingDto setSetting);
        public Task<RepoAnswer> DeleteSettingAsync(string key);
    }
}
