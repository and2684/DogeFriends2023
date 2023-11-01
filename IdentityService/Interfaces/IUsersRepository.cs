using DogeFriendsSharedClassLibrary;

namespace IdentityService.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserLoginResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserLoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> SeedRolesAsync();
    }
}
