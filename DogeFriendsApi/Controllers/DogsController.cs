using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogsRepository _dogRepository;

        public DogsController(IDogsRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        /// <summary>
        /// Получает список всех собак.
        /// </summary>
        /// <returns>Список всех собак.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDogs()
        {
            var (dogs, answerCode) = await _dogRepository.GetAllDogsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Собаки не найдены.");
                case RepoAnswer.Success:
                    return Ok(dogs);
                default:
                    return StatusCode(500, "Произошла ошибка при получении списка собак.");
            }
        }

        /// <summary>
        /// Получает информацию о собаке по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор собаки.</param>
        /// <returns>Информация о собаке.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDog(int id)
        {
            var (dog, answerCode) = await _dogRepository.GetDogAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Собака с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok(dog);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении информации о собаке с идентификатором {id}.");
            }
        }

        /// <summary>
        /// Создает новую собаку.
        /// </summary>
        /// <param name="dog">Модель с информацией о собаке.</param>
        /// <returns>Созданная собака.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDog([FromBody] DogAddOrUpdateDto dog)
        {
            var (newDog, answerCode) = await _dogRepository.CreateDogAsync(dog);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Собака уже существует.");
                case RepoAnswer.Success:
                    return Ok(newDog);
                default:
                    return StatusCode(500, "Произошла ошибка при создании собаки.");
            }
        }

        /// <summary>
        /// Обновляет информацию о собаке по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор собаки.</param>
        /// <param name="dog">Модель с обновленной информацией.</param>
        /// <returns>Обновленная информация о собаке.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDog(int id, [FromBody] DogAddOrUpdateDto dog)
        {
            var (updatedDog, answerCode) = await _dogRepository.UpdateDogAsync(id, dog);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Собака с именем {dog.Name} уже существует.");
                case RepoAnswer.NotFound:
                    return NotFound($"Собака с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok(updatedDog);
                default:
                    return StatusCode(500, "Произошла ошибка при обновлении информации о собаке.");
            }
        }

        /// <summary>
        /// Удаляет собаку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор собаки.</param>
        /// <returns>Результат удаления собаки.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDog(int id)
        {
            var answerCode = await _dogRepository.DeleteDogAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Собака с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"Произошла ошибка при удалении собаки с идентификатором {id}.");
            }
        }
    }
}