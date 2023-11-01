using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;

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

        // api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]RegisterDto registerDto)
        {
            if(ModelState.IsValid)
            {
                var result = await _userRepository.RegisterAsync(registerDto);
                if(result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Данные для регистрации пользователя не валидны.");
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
