using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DogeFriendsApi.Models
{
    [Table("Dogs")]
    [Index(nameof(UserId), nameof(Name), IsUnique = true)] // Имя собаки уникально в пределах каждого хозяина. Нельзя иметь двух псов с именем "Вася".
    public class Dog
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int BreedId { get; set; }
        public Breed? Breed { get; set; }
        [Required] // Собака не может быть без владельца
        public int UserId { get; set; }
        public User? User { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid(); // Для связи с внешними сервисами
    }
}
