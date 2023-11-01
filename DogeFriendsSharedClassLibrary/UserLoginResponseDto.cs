namespace DogeFriendsSharedClassLibrary
{
    public class UserLoginResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
