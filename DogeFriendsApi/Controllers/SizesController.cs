using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizesRepository _sizesRepository;

        public SizesController(ISizesRepository sizesRepository)
        {
            _sizesRepository = sizesRepository;
        }

        /// <summary>
        /// Получает все размеры пород собак.
        /// </summary>
        /// <returns>Список всех размеров пород собак.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSizes()
        {
            var (sizes, answerCode) = await _sizesRepository.GetAllSizesAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Размеры пород собак не найдены.");
                case RepoAnswer.Success:
                    return Ok(sizes);
                default:
                    return StatusCode(500, "Произошла ошибка при получении размеров пород собак.");
            }
        }

        /// <summary>
        /// Получает размер породы собаки по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор размера породы собаки.</param>
        /// <returns>Размер породы собаки.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSize(int id)
        {
            var (size, answerCode) = await _sizesRepository.GetSizeAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Размер породы собаки с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok(size);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении размера породы собаки с идентификатором {id}.");
            }
        }

        /// <summary>
        /// Создает новый размер породы собаки.
        /// </summary>
        /// <param name="size">Модель размера породы собаки.</param>
        /// <returns>Созданный размер породы собаки.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] SizeDto size)
        {
            var (newSize, answerCode) = await _sizesRepository.CreateSizeAsync(size);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Размер породы собаки уже существует.");
                case RepoAnswer.Success:
                    return Ok(newSize);
                default:
                    return StatusCode(500, "Произошла ошибка при создании размера породы собаки.");
            }
        }

        /// <summary>
        /// Обновляет размер породы собаки по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор размера породы собаки.</param>
        /// <param name="size">Модель размера породы собаки.</param>
        /// <returns>Обновленный размер породы собаки.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] SizeDto size)
        {
            var (updatedSize, answerCode) = await _sizesRepository.UpdateSizeAsync(id, size);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Размер породы собаки с именем {size.Name} уже существует.");
                case RepoAnswer.NotFound:
                    return NotFound($"Размер породы собаки с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok(updatedSize);
                default:
                    return StatusCode(500, "Произошла ошибка при обновлении размера породы собаки.");
            }
        }

        /// <summary>
        /// Удаляет размер породы собаки по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор размера породы собаки.</param>
        /// <returns>Результат удаления размера породы собаки.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var answerCode = await _sizesRepository.DeleteSizeAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Размер породы собаки с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"Произошла ошибка при удалении размера породы собаки с идентификатором {id}.");
            }
        }
    }
}
