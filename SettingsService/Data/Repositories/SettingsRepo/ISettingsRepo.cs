using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SettingsService.Models;

namespace SettingsService.Data.Repositories.SettingsRepo
{
    public interface ISettingsRepo
    {
        public Task<(string?, RepoAnswer)> GetSettingAsync(string key);
        public Task<(Setting?, RepoAnswer)> SetSettingAsync(string key, string value);
        public Task<RepoAnswer> DeleteSettingAsync(string key);
    }
}
