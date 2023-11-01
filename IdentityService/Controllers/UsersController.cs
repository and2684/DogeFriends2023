using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
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
    }
}
