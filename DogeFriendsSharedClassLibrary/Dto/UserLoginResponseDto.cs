﻿namespace DogeFriendsSharedClassLibrary.Dto
{
    public class UserLoginResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
