using Microsoft.AspNetCore.Identity;

namespace IdentityService.Interfaces
{
    public interface ITokenService
    {
        public Task<(string accessToken, string refreshToken)> GenerateTokenAsync(IdentityUser user);
        public Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task<bool> RemoveTokenForUserAsync(string userId);
        public Task<bool> ValidateAccessTokenAsync(string accessToken);
    }
}
