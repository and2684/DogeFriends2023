using Microsoft.AspNetCore.Mvc;
using SettingsService.Data;
using SettingsService.Data.Repositories.SettingsRepo;
using SettingsService.Models;

namespace SettingsService.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepo _settingsRepo;

        public SettingsController(ISettingsRepo settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetSetting(string key)
        {
            var (value, answerCode) = await _settingsRepo.GetSettingAsync(key);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Setting with key {key} not found.");
                case RepoAnswer.Success:
                    return Ok(value);
                default:
                    return StatusCode(500, $"An error occurred while retrieving the {key} value.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetSetting([FromBody] Setting setting)
        {
            var(settingSet, answerCode) = await _settingsRepo.SetSettingAsync(setting.Key, setting.Value);
            switch (answerCode)
            {
                case RepoAnswer.Success:
                    return Ok($"Setting successfuly saved. ({settingSet!.Key} : {settingSet.Value}).");
                case RepoAnswer.ActionFailed:
                    return BadRequest($"Bad request while setting the {setting.Key} value.");
                default:
                    return StatusCode(500, $"An error occurred while setting the {setting.Key} value.");
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteSetting(string key)
        {
            var answerCode = await _settingsRepo.DeleteSettingAsync(key);

            switch (answerCode)
            {
                case RepoAnswer.Success:
                    return Ok($"Setting successfuly deleted. ({key}).");
                default:
                    return StatusCode(500, $"An error occurred while deleting the {key} value.");
            }
        }
    }
}