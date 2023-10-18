﻿using DogeFriendsApi.Models;

namespace DogeFriendsApi.Dto
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Showname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Hometown { get; set; }
        public string? Description { get; set; }
    }
}
