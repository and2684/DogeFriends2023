using DogeFriendsSharedClassLibrary.Dto;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityController(IIdentityRepository userRepository)
        {
            _identityRepository = userRepository;
        }

        // api/users/login
        /// <summary>
        /// Вход пользователя.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto? loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Данные для входа пользователя не валидны.");

            var result = await _identityRepository.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/register
        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Данные для регистрации пользователя не валидны.");

            var result = await _identityRepository.RegisterAsync(registerDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/logout
        /// <summary>
        /// Выход пользователя.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUserAsync([FromBody] string username)
        {
            var result = await _identityRepository.LogoutAsync(username!);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/changepassword
        /// <summary>
        /// Изменение пароля пользователя.
        /// </summary>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
            {
                return BadRequest("Пароли совпадают");
            }

            var changePasswordResult = await _identityRepository.ChangePasswordAsync(changePasswordDto);

            if (changePasswordResult)
            {
                return Ok("Пароль успешно изменен");
            }

            return BadRequest("Не удалось изменить пароль");
        }


        // api/users/setrole
        /// <summary>
        /// Установка роли пользователю.
        /// </summary>
        [HttpPost("setrole")]
        public async Task<IActionResult> SetRoleAsync([FromHeader] string username, [FromHeader] string role)
        {
            var result = await _identityRepository.SetRoleAsync(username, role);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/removerole
        /// <summary>
        /// Удаление роли у пользователя.
        /// </summary>
        [HttpPost("removerole")]
        public async Task<IActionResult> RemoveRoleAsync([FromHeader] string username, [FromHeader] string role)
        {
            var result = await _identityRepository.RemoveRoleAsync(username, role);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //api/users/seed
        /// <summary>
        /// Заполнение базы IdentityServer начальными данными.
        /// </summary>
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            var result = await _identityRepository.SeedAsync();
            if (result)
                return Ok("Наполнение базы IdentityServer прошло успешно.");
            return BadRequest("Произошла ошибка при наполнении базы IdentityServer.");
        }

        //api/users/validatetoken
        /// <summary>
        /// Валидация токена.
        /// </summary>
        [HttpPost("validatetoken")]
        public async Task<IActionResult> ValidateToken([FromBody] string accessToken)
        {
            var result = await _identityRepository.ValidateTokenAsync(accessToken);
            if (result)
                return Ok("Токен валиден.");
            return BadRequest("Токен невалиден.");
        }

        // api/users/refreshtoken
        /// <summary>
        /// Обновление токена доступа.
        /// </summary>
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokensDto tokensDto)
        {
            var result = await _identityRepository.RefreshTokenAsync(tokensDto.AccessToken, tokensDto.RefreshToken);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
