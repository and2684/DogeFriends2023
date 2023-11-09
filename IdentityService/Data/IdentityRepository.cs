using DogeFriendsSharedClassLibrary.Dto;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using ApiScope = IdentityServer4.EntityFramework.Entities.ApiScope;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Data
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ITokenService _tokenService;

        public IdentityRepository(UserManager<IdentityUser> userManager, 
                                  RoleManager<IdentityRole> roleManager, 
                                  ConfigurationDbContext configurationDbContext,
                                  ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configurationDbContext = configurationDbContext;
            _tokenService = tokenService;
        }

        public async Task<UserLoginResponseDto> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var result = new UserLoginResponseDto();

            if (registerDto.Username.IsNullOrEmpty() || registerDto.Email.IsNullOrEmpty())
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
            var (accessToken, refreshToken) = await _tokenService.GenerateTokenAsync(identityUser);

            // Если юзера зарегили и токены успешно созданы - все ок
            if (identityResult.Succeeded && !accessToken.IsNullOrEmpty() && !refreshToken.IsNullOrEmpty())
            {
                await _userManager.AddToRoleAsync(identityUser, "User"); // Присвоение роли "User" новому пользователю

                result.AccessToken = accessToken;
                result.RefreshToken = refreshToken;
                result.Message = "Пользователь успешно зарегистрирован";
                result.IsSuccess = true;
                return result;
            }

            result.Message = "При регистрации пользователя возникла ошибка";
            result.Errors = identityResult.Errors.Select(x => x.Description).ToList();
            return result;
        }

        public async Task<UserLoginResponseDto> LoginAsync([FromBody] LoginDto? loginDto)
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

            var (accessToken, refreshToken) = await _tokenService.GenerateTokenAsync(user);

            if (!accessToken.IsNullOrEmpty() && !refreshToken.IsNullOrEmpty())
            {
                result.AccessToken = accessToken;
                result.RefreshToken = refreshToken;
                result.Message = "Аутентификация прошла успешно";
                result.IsSuccess = true;
                return result;
            }

            result.Message = "Ошибка генерации токенов";
            return result;
        }

        public async Task<UserLoginResponseDto> LogoutAsync(string username)
        {
            var result = new UserLoginResponseDto();

            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var removed = await _tokenService.RemoveTokenForUserAsync(user.Id);

                if (removed)
                {
                    result.Message = "Пользователь успешно вышел из системы.";
                    result.IsSuccess = true;
                    return result;
                }
            }

            result.Message = "Не удалось выполнить выход пользователя.";
            return result;
        }

        public async Task<UserLoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var result = new UserLoginResponseDto();

            var (newAccessToken, newRefreshToken) = await _tokenService.RefreshTokenAsync(accessToken, refreshToken);

            if (!string.IsNullOrEmpty(newAccessToken) && !string.IsNullOrEmpty(newRefreshToken))
            {
                result.AccessToken = newAccessToken;
                result.RefreshToken = newRefreshToken;
                result.Message = "Токены успешно обновлены";
                result.IsSuccess = true;
                return result;
            }

            result.Message = "Не удалось обновить токены";
            return result;
        }

        public async Task<bool> SetRoleAsync(string username, string role)
        {
            var identityUser = await _userManager.FindByNameAsync(username);
            if (identityUser == null) { return false; }
            var res = await _userManager.AddToRoleAsync(identityUser, role); 
            return res.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(string username, string role)
        {
            var identityUser = _userManager.FindByNameAsync(username).Result;
            if (identityUser == null) { return false; }
            var res = await _userManager.RemoveFromRoleAsync(identityUser, role);
            return res.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByNameAsync(changePasswordDto.Username);

            if (user != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    return true; // Пароль успешно изменен
                }
            }

            return false; // Не удалось изменить пароль пользователя
        }

        public async Task<bool> SeedAsync()
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

            var scopeExists = await _configurationDbContext.ApiScopes.AnyAsync(s => s.Name == "DogeFriendsDictionary");
            if (!scopeExists)
            {
                var writeScope = new ApiScope
                {
                    Name = "DogeFriendsDictionary.Write",
                    DisplayName = "Запись в справочники DogeFriends"
                };
                _configurationDbContext.ApiScopes.Add(writeScope);

                var readScope = new ApiScope
                {
                    Name = "DogeFriendsDictionary.Read",
                    DisplayName = "Чтение справочников DogeFriends"
                };
                _configurationDbContext.ApiScopes.Add(readScope);
            }

            var dataScopeExists = await _configurationDbContext.ApiScopes.AnyAsync(s => s.Name == "DogeFriendsData");
            if (!dataScopeExists)
            {
                var dataScope = new ApiScope
                {
                    Name = "DogeFriendsData",
                    DisplayName = "Доступ к рабочим данным DogeFriends"
                };
                _configurationDbContext.ApiScopes.Add(dataScope);
            }

            // Создание клиента
            var clientExists = await _configurationDbContext.Clients.AnyAsync(c => c.ClientId == "DogeFriendsApi");
            if (!clientExists)
            {
                var client = new IdentityServer4.EntityFramework.Entities.Client
                {
                    ClientId = "DogeFriendsApi",
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Scope = "DogeFriendsDictionary.Write" }, // Запись в справочники DogeFriends
                        new ClientScope { Scope = "DogeFriendsDictionary.Read" }, // Чтение справочников DogeFriends
                        new ClientScope { Scope = "DogeFriendsData" } // Доступ к рабочим данным DogeFriends
                    }
                };
                _configurationDbContext.Clients.Add(client);
            }
            await _configurationDbContext.SaveChangesAsync();

            return true;
        }

        // Валидация токена
        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await _tokenService.ValidateAccessTokenAsync(token);            
        }
    }
}
