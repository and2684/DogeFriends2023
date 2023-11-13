using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Models
{
    [Index(nameof(UserId), nameof(FriendId), IsUnique = true)] // Дружба не дублируется
    public class Friendship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int FriendId { get; set; }
        public User? Friend { get; set; }
    }
}
