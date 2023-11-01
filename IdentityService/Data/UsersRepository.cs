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

        public async Task<UserLoginResponseDto> RegisterAsync([FromBody] RegisterDto registerDto)
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

        public Task<bool> RemoveRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SeedRolesAsync()
        {
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                var role = new IdentityRole("Admin");
                await _roleManager.CreateAsync(role);
            }

            roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                var role = new IdentityRole("User");
                await _roleManager.CreateAsync(role);
            }

            roleExists = await _roleManager.RoleExistsAsync("ContentManager");
            if (!roleExists)
            {
                var role = new IdentityRole("ContentManager");
                await _roleManager.CreateAsync(role);
            }

            return true;
        }

        public Task<bool> SetRole(string username, string role)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ChangePasswordAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserLoginResponseDto> LoginAsync([FromBody] LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
