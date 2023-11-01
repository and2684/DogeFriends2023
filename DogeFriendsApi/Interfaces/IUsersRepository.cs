using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary;

namespace DogeFriendsApi.Interfaces
{
    public interface IUsersRepository
    {
        public Task<(IEnumerable<UserInfoDto>?, RepoAnswer)> GetAllUsersAsync();
        public Task<(UserInfoDto?, RepoAnswer)> GetUserByIdAsync(int id);
        public Task<(UserInfoDto?, RepoAnswer)> GetUserByUsernameAsync(string username);
        public Task<(UserInfoDto?, RepoAnswer)> UpdateUserAsync(UserInfoDto user);
        public Task<(UserLoginResponseDto?, RepoAnswer)> RegisterUserAsync(RegisterDto user);
        public Task<(UserLoginResponseDto?, RepoAnswer)> LoginUserAsync(LoginDto user);
    }
}
