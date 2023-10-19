using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface IFriendshipsRepository
    {
        public Task<RepoAnswer> CreateFriendshipAsync(int userId, int friendId);
        public Task<RepoAnswer> RemoveFriendshipAsync(int userId, int friendId);
        public Task<(List<UserInfoDto>?, RepoAnswer)> GetFriendsAsync(int userId);
    }
}