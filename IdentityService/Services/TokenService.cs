using DogeFriendsSharedClassLibrary.Interfaces;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityServer4.Models;
using ITokenService = IdentityService.Interfaces.ITokenService;

namespace IdentityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISettingsService _settingsService;
        private readonly IPersistedGrantStore _persistedGrantStore;
        private string? _encryptionKey;
        private string? _audience;
        private string? _issuer;
        private string? _secret;
        private string? _clientId;

        public TokenService(UserManager<IdentityUser> userManager,
                            ISettingsService settingsService,
                            IConfiguration config,
                            IPersistedGrantStore persistedGrantStore)
        {
            _userManager = userManager;
            _settingsService = settingsService;
            _persistedGrantStore = persistedGrantStore;
            InitializeSettings(config).GetAwaiter().GetResult();
        }

        private async Task InitializeSettings(IConfiguration config)
        {
            // Инициализация настроек JWT
            _encryptionKey = config.GetSection("SettingsService:SettingsEncryptionKey").Value!;
            _audience = (await _settingsService.GetSettingValue(config.GetSection("SettingsService:Audience").Value!, _encryptionKey))!;
            _issuer = (await _settingsService.GetSettingValue(config.GetSection("SettingsService:Issuer").Value!, _encryptionKey))!;
            _secret = (await _settingsService.GetSettingValue(config.GetSection("SettingsService:Secret").Value!, _encryptionKey))!;
            _clientId = (await _settingsService.GetSettingValue(config.GetSection("SettingsService:ClientId").Value!, _encryptionKey))!;
        }

        public async Task StoreTokensAsync(string accessToken, string refreshToken, string subjectId, string clientId)
        {
            var accessTokenGrant = new PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = "access_token",
                SubjectId = subjectId, // subjectId - это юзер
                ClientId = clientId,
                CreationTime = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddHours(1), // Настройте срок действия токена
                Data = accessToken // Сохранение самого токена
            };

            var refreshTokenGrant = new PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = "refresh_token",
                SubjectId = subjectId,
                ClientId = clientId,
                CreationTime = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1), // Настройте срок действия обновления токена
                Data = refreshToken // Сохранение обновления токена
            };

            await _persistedGrantStore.StoreAsync(accessTokenGrant);
            await _persistedGrantStore.StoreAsync(refreshTokenGrant);
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokenAsync(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            // Добавление ролей в claims
            var userRoles = await _userManager.GetRolesAsync(user); 
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessJwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(accessJwt);

            var refreshJwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );
            var refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

            await RemoveTokenForUserAsync(user.Id); // Старые токены удаляем
            await StoreTokensAsync(accessToken, refreshToken, user.Id, _clientId!);

            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var allRefreshTokens = await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter { Type = "refresh_token" });
            var storedRefreshToken = allRefreshTokens.FirstOrDefault(token => token.Data == refreshToken);

            if (storedRefreshToken != null && storedRefreshToken.Data == refreshToken)
            {
                var user = await _userManager.FindByIdAsync(storedRefreshToken.SubjectId);

                if (user != null)
                {
                    var (newAccessToken, newRefreshToken) = await GenerateTokenAsync(user);

                    await _persistedGrantStore.RemoveAsync(refreshToken);
                    await _persistedGrantStore.RemoveAsync(accessToken);
                    await _persistedGrantStore.StoreAsync(new PersistedGrant
                    {
                        Key = Guid.NewGuid().ToString(),
                        Type = "refresh_token",
                        SubjectId = user.Id,
                        ClientId = _clientId,
                        CreationTime = DateTime.UtcNow,
                        Expiration = DateTime.UtcNow.AddDays(30), // Новый срок действия refresh token
                        Data = newRefreshToken
                    });
                    await _persistedGrantStore.StoreAsync(new PersistedGrant
                    {
                        Key = Guid.NewGuid().ToString(),
                        Type = "access_token",
                        SubjectId = user.Id,
                        ClientId = _clientId,
                        CreationTime = DateTime.UtcNow,
                        Expiration = DateTime.UtcNow.AddHours(1), // Новый срок действия access token
                        Data = newAccessToken
                    });

                    return (newAccessToken, newRefreshToken);
                }
            }

            // Если обновление не удалось, возвращаем пустые токены
            return (string.Empty, string.Empty);
        }

        public async Task<bool> RemoveTokenForUserAsync(string userId)
        {
            var userTokens = await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter { SubjectId = userId });

            if (userTokens == null || !userTokens.Any()) return false; // Не найдены токены для удаления

            foreach (var token in userTokens)
            {
                await _persistedGrantStore.RemoveAsync(token.Key);
            }

            return true; // Токены пользователя успешно удалены
        }

        public async Task<bool> ValidateAccessTokenAsync(string accessToken)
        {
            var persistedGrants = await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter { Type = "access_token" });

            var tokenInDb = persistedGrants.FirstOrDefault(grant => grant.Data == accessToken);

            if (tokenInDb == null) return false;
            return tokenInDb.Expiration > DateTime.UtcNow;
        }
    }
}