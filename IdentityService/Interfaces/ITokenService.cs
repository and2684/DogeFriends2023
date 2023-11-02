using Microsoft.AspNetCore.Identity;

namespace IdentityService.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(IdentityUser user);
    }
}
