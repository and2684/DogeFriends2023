namespace DogeFriendsSharedClassLibrary.Dto
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Showname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Hometown { get; set; }
        public string? Description { get; set; }
        public string ExternalId { get; set; } = string.Empty;
    }
}
