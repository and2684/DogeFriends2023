using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoatsController : ControllerBase
    {
        private readonly ICoatsRepository _coatsRepository;

        public CoatsController(ICoatsRepository coatsRepository)
        {
            _coatsRepository = coatsRepository;
        }

        /// <summary>
        /// Получает все виды шерсти собак.
        /// </summary>
        /// <returns>Список всех видов шерсти.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCoats()
        {
            var (coats, answerCode) = await _coatsRepository.GetAllCoatsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Виды шерсти не найдены.");
                case RepoAnswer.Success:
                    return Ok(coats);
                default:
                    return StatusCode(500, "Произошла ошибка при получении видов шерсти.");
            }
        }

        /// <summary>
        /// Получает вид шерсти собаки по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор вид шерсти.</param>
        /// <returns>Информация о виде шерсти.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCoat(int id)
        {
            var (coat, answerCode) = await _coatsRepository.GetCoatAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Вид шерсти с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok(coat);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении информации о виде шерсти с идентификатором {id}.");
            }
        }

        /// <summary>
        /// Создает новый вид шерсти собаки.
        /// </summary>
        /// <param name="coat">Модель с информацией о виде шерсти.</param>
        /// <returns>Созданный вид шерсти.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCoat([FromBody] CoatDto coat)
        {
            var (newCoat, answerCode) = await _coatsRepository.CreateCoatAsync(coat);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Вид шерсти уже существует.");
                case RepoAnswer.Success:
                    return Ok(newCoat);
                default:
                    return StatusCode(500, "Произошла ошибка при создании вида шерсти.");
            }
        }

        /// <summary>
        /// Обновляет информацию о виде шерсти собаки.
        /// </summary>
        /// <param name="id">Идентификатор вид шерсти.</param>
        /// <param name="coat">Модель с обновленной информацией.</param>
        /// <returns>Обновленная информация о виде шерсти.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCoat(int id, [FromBody] CoatDto coat)
        {
            var (updatedCoat, answerCode) = await _coatsRepository.UpdateCoatAsync(id, coat);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Вид шерсти с именем {coat.Name} уже существует.");
                case RepoAnswer.NotFound:
                    return NotFound($"Вид шерсти с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok(updatedCoat);
                default:
                    return StatusCode(500, "Произошла ошибка при обновлении информации о виде шерсти.");
            }
        }

        /// <summary>
        /// Удаляет вид шерсти собаки по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор вид шерсти.</param>
        /// <returns>Результат удаления вид шерсти.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCoat(int id)
        {
            var answerCode = await _coatsRepository.DeleteCoatAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Вид шерсти с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"Произошла ошибка при удалении вид шерсти с идентификатором {id}.");
            }
        }
    }
}