﻿using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedsRepository _breedsRepository;

        public BreedsController(IBreedsRepository breedsRepository)
        {
            _breedsRepository = breedsRepository;
        }

        /// <summary>
        /// Получает список всех пород собак.
        /// </summary>
        /// <returns>Список всех пород собак.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBreeds()
        {
            var (breeds, answerCode) = await _breedsRepository.GetAllBreedsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Породы не найдены.");
                case RepoAnswer.Success:
                    return Ok(breeds);
                default:
                    return StatusCode(500, "Произошла ошибка при получении списка пород собак.");
            }
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
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Порода с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok(breed);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении информации о породе собаки с идентификатором {id}.");
            }
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
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Порода собаки уже существует.");
                case RepoAnswer.Success:
                    return Ok(newBreed);
                default:
                    return StatusCode(500, "Произошла ошибка при создании породы собаки.");
            }
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
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Порода с именем {breed.Name} уже существует.");
                case RepoAnswer.NotFound:
                    return NotFound($"Порода с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok(updatedBreed);
                default:
                    return StatusCode(500, "Произошла ошибка при обновлении породы собаки.");
            }
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
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Порода с идентификатором {id} не найдена.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"Произошла ошибка при удалении породы собаки с идентификатором {id}.");
            }
        }
    }
}