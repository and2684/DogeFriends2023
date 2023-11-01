using DogeFriendsSharedClassLibrary;

namespace IdentityService.Interfaces
{
    public interface IUserRepository
    {
        Task<UserLoginResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserLoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
