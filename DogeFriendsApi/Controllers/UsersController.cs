using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;
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
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound("Пользователи не найдены."),
                RepoAnswer.Success => Ok(users),
                _ => StatusCode(500, "Произошла ошибка при получении пользователей."),
            };
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
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Пользователь с идентификатором {id} не найден."),
                RepoAnswer.Success => Ok(user),
                _ => StatusCode(500, $"Произошла ошибка при получении пользователя с идентификатором {id}."),
            };
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
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Имя пользователя {username} не найдено."),
                RepoAnswer.Success => Ok(user),
                _ => StatusCode(500, $"Произошла ошибка при получении пользователя {username}."),
            };
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
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Имя пользователя {user.Username} не найдено."),
                RepoAnswer.EmailTaken => Conflict($"Email {user.Email} уже занят."),
                RepoAnswer.ActionFailed => BadRequest($"Произошла ошибка при обновлении информации о пользователе {user.Username}."),
                RepoAnswer.Success => Ok(updatedUser),
                _ => StatusCode(500, $"Произошла ошибка при обновлении информации о пользователе {user.Username}."),
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            var (identityAnswer, answerCode) = await _usersRepository.RegisterUserAsync(user);
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Страница не найдена."),
                RepoAnswer.UsernameTaken => Conflict($"Имя пользователя {user.Username} уже занято."),
                RepoAnswer.EmailTaken => Conflict($"Email {user.Email} уже занят."),
                RepoAnswer.ActionFailed when identityAnswer?.Errors?.Any() == true => BadRequest($"Некорректный запрос на регистрацию. ({string.Join(", ", identityAnswer.Errors)})"),
                RepoAnswer.ActionFailed when !string.IsNullOrEmpty(identityAnswer?.Message) => BadRequest($"Некорректный запрос на регистрацию. ({identityAnswer.Message})"),
                RepoAnswer.ActionFailed => BadRequest("Некорректный запрос на регистрацию, но не были предоставлены дополнительные сведения об ошибке."),
                RepoAnswer.Success => Ok(identityAnswer),
                _ => StatusCode(500, $"Произошла ошибка при регистрации пользователя {user.Username}."),
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto user)
        {
            var (identityAnswer, answerCode) = await _usersRepository.LoginUserAsync(user);
            return answerCode switch
            {
                RepoAnswer.NotFound => NotFound($"Пользователь {user.Username} не найден."),
                RepoAnswer.ActionFailed => BadRequest($"Некорректный запрос на аутентификацию. ({identityAnswer?.Message})"),
                RepoAnswer.Success => Ok(identityAnswer),
                _ => StatusCode(500, $"Произошла ошибка при аутентификации пользователя {user.Username}."),
            };
        }
    }
}
