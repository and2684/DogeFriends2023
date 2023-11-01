using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <returns>Список всех пользователей.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var (users, answerCode) = await _usersRepository.GetAllUsersAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("Пользователи не найдены.");
                case RepoAnswer.Success:
                    return Ok(users);
                default:
                    return StatusCode(500, "Произошла ошибка при получении пользователей.");
            }
        }

        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var (user, answerCode) = await _usersRepository.GetUserByIdAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Пользователь с идентификатором {id} не найден.");
                case RepoAnswer.Success:
                    return Ok(user);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении пользователя с идентификатором {id}.");
            }
        }

        /// <summary>
        /// Получает пользователя по его имени пользователя (username).
        /// </summary>
        /// <param name="username">Имя пользователя (username).</param>
        /// <returns>Пользователь.</returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var (user, answerCode) = await _usersRepository.GetUserByUsernameAsync(username);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Имя пользователя {username} не найдено.");
                case RepoAnswer.Success:
                    return Ok(user);
                default:
                    return StatusCode(500, $"Произошла ошибка при получении пользователя {username}.");
            }
        }

        /// <summary>
        /// Обновляет информацию о пользователе.
        /// </summary>
        /// <param name="user">Модель пользователя с обновленными данными.</param>
        /// <returns>Обновленный пользователь.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserInfoDto user)
        {
            var (updatedUser, answerCode) = await _usersRepository.UpdateUserAsync(user);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Имя пользователя {user.Username} не найдено.");
                case RepoAnswer.EmailTaken:
                    return Conflict($"Email {user.Email} уже занят.");
                case RepoAnswer.ActionFailed:
                    return BadRequest($"Произошла ошибка при обновлении информации о пользователе {user.Username}.");
                case RepoAnswer.Success:
                    return Ok(updatedUser);
                default:
                    return StatusCode(500, $"Произошла ошибка при обновлении информации о пользователе {user.Username}.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            var (identityAnswer, answerCode) = await _usersRepository.RegisterUserAsync(user);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Страница не найдена.");
                case RepoAnswer.UsernameTaken:
                    return Conflict($"Имя пользователя {user.Username} уже занято.");
                case RepoAnswer.EmailTaken:
                    return Conflict($"Email {user.Email} уже занят.");
                case RepoAnswer.ActionFailed:
                    if (identityAnswer?.Errors != null && identityAnswer.Errors.Any())
                    {
                        var errorsMessage = string.Join(", ", identityAnswer.Errors);
                        return BadRequest($"Некорректный запрос на регистрацию. ({errorsMessage})");
                    }
                    if (!string.IsNullOrEmpty(identityAnswer?.Message))
                        return BadRequest($"Некорректный запрос на регистрацию. ({identityAnswer.Message})");
                    return BadRequest("Некорректный запрос на регистрацию, но не были предоставлены дополнительные сведения об ошибке.");
                case RepoAnswer.Success:
                    return Ok(identityAnswer);
                default:
                    return StatusCode(500, $"Произошла ошибка при регистрации пользователя {user.Username}.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto user)
        {
            var (identityAnswer, answerCode) = await _usersRepository.LoginUserAsync(user);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Пользователь {user.Username} не найден.");
                case RepoAnswer.ActionFailed:
                    return BadRequest($"Некорректный запрос на аутентификацию. ({identityAnswer?.Message})");
                case RepoAnswer.Success:
                    return Ok(identityAnswer);
                default:
                    return StatusCode(500, $"Произошла ошибка при аутентификации пользователя {user.Username}.");
            }
        }
    }
}
