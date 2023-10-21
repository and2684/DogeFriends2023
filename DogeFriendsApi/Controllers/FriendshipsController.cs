using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipsRepository _friendshipsRepository;

        public FriendshipsController(IFriendshipsRepository friendshipsRepository)
        {
            _friendshipsRepository = friendshipsRepository;
        }

        /// <summary>
        /// Получает информацию о дружбе пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список друзей пользователя.</returns>
        [HttpGet]
        public async Task<IActionResult> GetFriendships(int userId)
        {
            var (friendships, answerCode) = await _friendshipsRepository.GetFriendsAsync(userId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Друзей не найдено. Это грустно.");
                case RepoAnswer.Success:
                    return Ok(friendships);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении списка друзей.");
            }
        }

        /// <summary>
        /// Создает дружбу между пользователями.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="friendId">Идентификатор друга.</param>
        /// <returns>Результат создания дружбы.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateFriendship(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.CreateFriendshipAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Идентификатор пользователя или друга не найден.");
                case RepoAnswer.Success:
                    return Ok("Теперь вы друзья <3");
                default:
                    return StatusCode(500, $"Произошла ошибка при установлении дружбы.");
            }
        }

        /// <summary>
        /// Разрывает дружбу между пользователями.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="friendId">Идентификатор друга.</param>
        /// <returns>Результат разрыва дружбы.</returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveFriendship(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.RemoveFriendshipAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Идентификатор пользователя или друга не найден.");
                case RepoAnswer.Success:
                    return Ok("Дружбы больше нет. :(");
                default:
                    return StatusCode(500, $"Произошла ошибка при отмене дружбы.");
            }
        }
    }
}
