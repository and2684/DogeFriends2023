﻿using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface IUsersRepository
    {
        public Task<(IEnumerable<UserInfoDto>?, RepoAnswer)> GetAllUsersAsync();
        public Task<(UserInfoDto?, RepoAnswer)> GetUserByIdAsync(int id);
        public Task<(UserInfoDto?, RepoAnswer)> GetUserByUsernameAsync(string username);
        public Task<(UserInfoDto?, RepoAnswer)> UpdateUserAsync(User user);
        public Task<RepoAnswer> CreateFriendshipAsync(int userId, int friendId);
        public Task<RepoAnswer> RemoveFriendshipAsync(int userId, int friendId);
    }
}