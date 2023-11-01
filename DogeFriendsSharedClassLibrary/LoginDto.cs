using System.ComponentModel.DataAnnotations;

namespace DogeFriendsSharedClassLibrary
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}
