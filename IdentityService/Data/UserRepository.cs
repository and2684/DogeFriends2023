using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Data
{
    public class UserRepository : IUserRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<UserLoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public async Task<UserLoginResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
                throw new NullReferenceException("Нет данных для регистрации пользователя.");

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new UserLoginResponseDto
                {
                    Message = "Пароль и подтверждение не совпадают.",
                    IsSuccess = false
                };
            }

            var identityUser = new IdentityUser
            {
                Email = registerDto.Email.ToLower(),
                UserName = registerDto.Username.ToLower()
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserLoginResponseDto
                {
                    Message = "Пользователь успешно зарегистрирован",
                    IsSuccess = true
                };
            }

            return new UserLoginResponseDto
            {
                Message = "При регистрации пользователя возникла ошибка",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}
