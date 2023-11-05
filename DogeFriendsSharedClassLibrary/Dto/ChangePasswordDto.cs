using System.ComponentModel.DataAnnotations;

namespace DogeFriendsSharedClassLibrary.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
