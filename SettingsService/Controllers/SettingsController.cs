using Microsoft.AspNetCore.Mvc;
using SettingsService.Data.Models;
using SettingsService.Services.Interfaces;

namespace SettingsService.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository _settingsRepo;

        public SettingsController(ISettingsRepository settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        /// <summary>
        /// Получает все настройки.
        /// </summary>
        /// <returns>Список всех настроек</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            var (settings, answerCode) = await _settingsRepo.GetAllSettingsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Settings not found.");
                case RepoAnswer.Success:
                    return Ok(settings);
                default:
                    return StatusCode(500, "An error occurred while retrieving the settings.");
            }
        }

        /// <summary>
        /// Получает настройку по ключу.
        /// </summary>
        /// <param name="key">Ключ настройки.</param>
        /// <returns>Значение настройки.</returns>
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

        /// <summary>
        /// Устанавливает настройку.
        /// </summary>
        /// <param name="setting">Модель настройки.</param>
        /// <returns>Результат установки настройки.</returns>
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

        /// <summary>
        /// Удаляет настройку по ключу.
        /// </summary>
        /// <param name="key">Ключ настройки.</param>
        /// <returns>Результат удаления настройки.</returns>
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