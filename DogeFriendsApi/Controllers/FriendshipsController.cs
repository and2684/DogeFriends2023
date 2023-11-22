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
        /// Получает информацию о друзьях пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список друзей пользователя.</returns>
        [HttpGet("friends")]
        public async Task<IActionResult> GetFriendships(int userId)
        {
            var (friendships, answerCode) = await _friendshipsRepository.GetFriendsAsync(userId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("У вас нет друзей. Это грустно.");
                case RepoAnswer.Success:
                    return Ok(friendships);
                default:
                    return StatusCode(500, "Произошла ошибка при получении списка друзей.");
            }
        }

        /// <summary>
        /// Получает информацию о подписчиках пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список подписчиков пользователя.</returns>
        [HttpGet("subs")]
        public async Task<IActionResult> GetSubscribers(int userId)
        {
            var (friendships, answerCode) = await _friendshipsRepository.GetSubscribersAsync(userId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("На вас никто не подписан");
                case RepoAnswer.Success:
                    return Ok(friendships);
                default:
                    return StatusCode(500, "Произошла ошибка при получении списка подписчиков.");
            }
        }

        /// <summary>
        /// Получает информацию о подписках пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список подписок пользователя.</returns>
        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetSubscriptions(int userId)
        {
            var (friendships, answerCode) = await _friendshipsRepository.GetSubscribtionsAsync(userId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Вы ни на кого не подписаны");
                case RepoAnswer.Success:
                    return Ok(friendships);
                default:
                    return StatusCode(500, "Произошла ошибка при получении списка подписок.");
            }
        }

        /// <summary>
        /// Подписка.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="friendId">Идентификатор подписки.</param>
        /// <returns>Результат подписки.</returns>
        [HttpPost("sub")]
        public async Task<IActionResult> Subscribe(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.SubscribeAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Идентификатор пользователя или подписки не найден.");
                case RepoAnswer.AlreadyExist:
                    return BadRequest("Вы уже подписаны.");
                case RepoAnswer.Success:
                    return Ok("Подписка успешно выполнена.");
                default:
                    return StatusCode(500, "Произошла ошибка при подписке.");
            }
        }

        /// <summary>
        /// Отписка.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="friendId">Идентификатор подписки.</param>
        /// <returns>Результат отписки.</returns>
        [HttpDelete("unsub")]
        public async Task<IActionResult> Unsubscribe(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.UnsubscribeAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Идентификатор пользователя или подписки не найден.");
                case RepoAnswer.Success:
                    return Ok("Подписка отменена");
                default:
                    return StatusCode(500, "Произошла ошибка при отмене подписки.");
            }
        }
    }
}
