using DogeFriendsSharedClassLibrary.Interfaces;
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

        public TokenService(ISettingsService settingsService, IConfiguration config) 
        {
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("SettingsService:Secret").Value!));
        }

        public async Task<string> GenerateTokenAsync(ClaimsIdentity claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1), // Время жизни токена (здесь - 1 час)
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}