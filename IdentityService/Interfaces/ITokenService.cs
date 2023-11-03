using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityService.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ClaimsIdentity claims);
    }
}
