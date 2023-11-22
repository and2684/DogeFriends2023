using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;

namespace DogeFriendsApi.Interfaces
{
    public interface IFriendshipsRepository
    {
        public Task<RepoAnswer> SubscribeAsync(int userId, int friendId);
        public Task<RepoAnswer> UnsubscribeAsync(int userId, int friendId);
        public Task<(List<UserInfoDto>?, RepoAnswer)> GetFriendsAsync(int userId);
        public Task<(List<UserInfoDto>?, RepoAnswer)> GetSubscribersAsync(int userId);
        public Task<(List<UserInfoDto>?, RepoAnswer)> GetSubscribtionsAsync(int userId);
    }
}