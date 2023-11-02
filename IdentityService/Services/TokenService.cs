using DogeFriendsApi.Interfaces;
using IdentityService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _signingKey; 

        public TokenService(ISettingsService settingsService) 
        {
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settingsService.GetSettingValue("DogeFriendsSecretKey", "DogeFriendsEncryptionKey")));
        }

        public async Task<string> GenerateTokenAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    // Другие необходимые клеймы
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Время жизни токена (здесь - 1 час)
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}