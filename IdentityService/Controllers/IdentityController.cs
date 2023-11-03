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
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto loginDto)
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

        //api/users/seedroles
        [HttpPost("seedroles")]
        public async Task<IActionResult> SeedRoles()
        {
            var result = await _identityRepository.SeedRolesAsync();
            if (result)
                return Ok("Роли добавлены успешно.");
            return BadRequest("Произошла ошибка при добавлении ролей.");
        }
    }
}
