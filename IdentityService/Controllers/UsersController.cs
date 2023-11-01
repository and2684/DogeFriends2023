using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _userRepository;

        public UsersController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Данные для входа пользователя не валидны.");

            var result = await _userRepository.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Данные для регистрации пользователя не валидны.");

            var result = await _userRepository.RegisterAsync(registerDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //api/users/seedroles
        [HttpPost("seedroles")]
        public async Task<IActionResult> SeedRoles()
        {
            var result = await _userRepository.SeedRolesAsync();
            if (result)
                return Ok("Роли добавлены успешно.");
            return BadRequest("Произошла ошибка при добавлении ролей.");
        }
    }
}
