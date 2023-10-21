using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DogeFriendsApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class BreedGroup
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<Breed>? Breeds { get; set; }
    }
}
