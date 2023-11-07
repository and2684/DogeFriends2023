using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedsRepository _breedsRepository;
        private readonly ILoggerManager _logger;

        public BreedsController(IBreedsRepository breedsRepository, ILoggerManager logger)
        {
            _breedsRepository = breedsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает список всех пород собак.
        /// </summary>
        /// <returns>Список всех пород собак.</returns>
        [Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBreeds()
        {
            var (breeds, answerCode) = await _breedsRepository.GetAllBreedsAsync();
            _logger.LogDebug( $"Получен список всех пород собак.");
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound("Породы не найдены."),
                RepoAnswer.Success => Ok(breeds),
                _ => StatusCode(500, "Произошла ошибка при получении списка пород собак.")
            };
        }

        /// <summary>
        /// Получает информацию о породе собаки по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор породы собаки.</param>
        /// <returns>Информация о породе собаки.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBreed(int id)
        {
            var (breed, answerCode) = await _breedsRepository.GetBreedAsync(id);
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Порода с идентификатором {id} не найдена."),
                RepoAnswer.Success => Ok(breed),
                _ => StatusCode(500,
                    $"Произошла ошибка при получении информации о породе собаки с идентификатором {id}.")
            };
        }

        /// <summary>
        /// Создает новую породу собаки.
        /// </summary>
        /// <param name="breed">Модель данных для создания породы собаки.</param>
        /// <returns>Созданная порода собаки.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBreed([FromBody] BreedDto breed)
        {
            var (newBreed, answerCode) = await _breedsRepository.CreateBreedAsync(breed);
            return answerCode switch
            {
                RepoAnswer.AlreadyExist => Conflict("Порода собаки уже существует."),
                RepoAnswer.Success => Ok(newBreed),
                _ => StatusCode(500, "Произошла ошибка при создании породы собаки.")
            };
        }

        /// <summary>
        /// Обновляет информацию о породе собаки по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор породы собаки.</param>
        /// <param name="breed">Модель данных для обновления породы собаки.</param>
        /// <returns>Обновленная информация о породе собаки.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBreed(int id, [FromBody] BreedDto breed)
        {
            var (updatedBreed, answerCode) = await _breedsRepository.UpdateBreedAsync(id, breed);
            return answerCode switch
            {
                RepoAnswer.AlreadyExist => Conflict($"Порода с именем {breed.Name} уже существует."),
                RepoAnswer.NotFound => NotFound($"Порода с идентификатором {id} не найдена."),
                RepoAnswer.Success => Ok(updatedBreed),
                _ => StatusCode(500, "Произошла ошибка при обновлении породы собаки.")
            };
        }

        /// <summary>
        /// Удаляет породу собаки по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор породы собаки для удаления.</param>
        /// <returns>Результат удаления породы собаки.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBreed(int id)
        {
            var answerCode = await _breedsRepository.DeleteBreedAsync(id);
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Порода с идентификатором {id} не найдена."),
                RepoAnswer.Success => Ok(),
                _ => StatusCode(500, $"Произошла ошибка при удалении породы собаки с идентификатором {id}.")
            };
        }
    }
}