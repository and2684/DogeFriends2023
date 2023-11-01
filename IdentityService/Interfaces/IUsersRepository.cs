using DogeFriendsSharedClassLibrary;

namespace IdentityService.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserLoginResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserLoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ChangePasswordAsync(LoginDto loginDto); // Для смены пароля воспользуемся тем же loginDto, что и при аутентификации
        Task<bool> SetRole(string username, string role);
        Task<bool> RemoveRole(string username, string role);
        Task<bool> SeedRolesAsync();
    }
}
