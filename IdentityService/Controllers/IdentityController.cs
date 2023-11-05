using DogeFriendsSharedClassLibrary.Dto;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUserAsync([FromBody] string username)
        {
            var result = await _identityRepository.LogoutAsync(username);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/changepassword
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

        // api/users/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync([FromHeader] string accessToken, [FromHeader] string refreshToken)
        {
            var result = await _identityRepository.RefreshTokenAsync(accessToken, refreshToken);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/users/setrole
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
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            var result = await _identityRepository.SeedAsync();
            if (result)
                return Ok("Наполнение базы IdentityServer прошло успешно.");
            return BadRequest("Произошла ошибка при наполнении базы IdentityServer.");
        }
    }
}
