using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Data
{
    public class UsersRepository : IUsersRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<UserLoginResponseDto> LoginAsync([FromBody]LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public async Task<UserLoginResponseDto> RegisterAsync([FromBody]RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return new UserLoginResponseDto
                {
                    Message = "Нет данных для регистрации пользователя.",
                    IsSuccess = false
                };
            }

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
