using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? Hometown { get; set; }
        public string? Description { get; set; }
        public List<Dog>? Dogs { get; set; }
        public List<Friendship>? FriendOf { get; set; }
        public List<Friendship>? Friends { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid(); // Для связи с внешними сервисами
    }
}