using Microsoft.AspNetCore.Mvc;
using SettingsService.Data.Models;
using SettingsService.Dto;
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
        /// <returns>Список всех настроек.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSettings()
        {
            var (settings, answerCode) = await _settingsRepo.GetAllSettingsAsync();
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound("Настройки не найдены."),
                RepoAnswer.Success => Ok(settings),
                _ => StatusCode(500, "Произошла ошибка при получении настроек.")
            };
        }

        /// <summary>
        /// Получает настройку по ключу.
        /// </summary>
        /// <param name="getSetting">Модель получения настройки.</param>
        /// <returns>Значение настройки.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSetting([FromQuery] GetSettingDto getSetting)
        {
            var (value, answerCode) = await _settingsRepo.GetSettingAsync(getSetting);
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Настройка с ключом {getSetting.Key} не найдена."),
                RepoAnswer.Success => Ok(value),
                _ => StatusCode(500, $"Произошла ошибка при получении значения настройки {getSetting.Key}.")
            };
        }

        /// <summary>
        /// Устанавливает настройку.
        /// </summary>
        /// <param name="setSetting">Модель настройки.</param>
        /// <returns>Результат установки настройки.</returns>
        [HttpPost]
        public async Task<IActionResult> SetSetting([FromBody] SetSettingDto setSetting)
        {
            var (settingSet, answerCode) = await _settingsRepo.SetSettingAsync(setSetting);
            return answerCode switch
            {
                RepoAnswer.Success => Ok($"Настройка успешно сохранена. ({settingSet!.Key} : {settingSet.Value})."),
                RepoAnswer.ActionFailed => BadRequest($"Неверный запрос при установке значения {setSetting.Key}."),
                _ => StatusCode(500, $"Произошла ошибка при установке значения {setSetting.Key}.")
            };
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

            return answerCode switch
            {
                RepoAnswer.Success => Ok($"Настройка успешно удалена. ({key})."),
                _ => StatusCode(500, $"Произошла ошибка при удалении значения {key}.")
            };
        }
    }
}
