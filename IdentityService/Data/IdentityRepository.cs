using DogeFriendsSharedClassLibrary;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Data
{
    public class IdentityRepository : IIdentityRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public IdentityRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserLoginResponseDto> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var result = new UserLoginResponseDto();

            if (registerDto == null)
            {
                result.Message = "Нет данных для регистрации пользователя.";
                return result;
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                result.Message = "Пароль и подтверждение не совпадают.";
                return result;
            };

            var identityUser = new IdentityUser
            {
                Email = registerDto.Email.ToLower(),
                UserName = registerDto.Username.ToLower()
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerDto.Password);

            if (identityResult.Succeeded)
            {
                result.Message = "Пользователь успешно зарегистрирован";
                result.IsSuccess = true;
                return result;
            }

            result.Message = "При регистрации пользователя возникла ошибка";
            result.Errors = identityResult.Errors.Select(x => x.Description).ToList();
            return result;
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

        public async Task<UserLoginResponseDto> LoginAsync([FromBody] LoginDto loginDto)
        {
            var result = new UserLoginResponseDto();

            if (loginDto == null)
            {
                result.Message = "Нет данных для входа.";
                return result;
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                result.Message = "Пользователь с таким именем не найден";
                return result;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                result.Message = "Неправильный пароль";
                return result;
            }

            result.Message = "Аутентификация прошла успешно";
            result.IsSuccess = true;
            return result;
        }

        public async Task<bool> SetRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePasswordAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
