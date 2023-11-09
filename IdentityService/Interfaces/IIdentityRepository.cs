using DogeFriendsSharedClassLibrary.Dto;

namespace IdentityService.Interfaces
{
    public interface IIdentityRepository
    {
        Task<UserLoginResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserLoginResponseDto> LoginAsync(LoginDto? loginDto);
        Task<UserLoginResponseDto> LogoutAsync(string username);
        Task<UserLoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<bool> SetRoleAsync(string username, string role);
        Task<bool> RemoveRoleAsync(string username, string role);
        Task<bool> SeedAsync();
        Task<bool> ValidateTokenAsync(string token); 
    }
}
