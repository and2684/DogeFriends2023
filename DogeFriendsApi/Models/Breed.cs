using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DogeFriendsApi.Models
{
    [Index(nameof(BreedName), IsUnique = true)]
    public class Breed
    {
        public int Id { get; set; }
        [Required]
        public string BreedName { get; set; } = string.Empty;
        public string BreedNameRu { get; set; } = string.Empty;
        public List<Dog>? Dogs { get; set; }
        // Тип шерсти
        public int CoatId { get; set; }
        public Coat? Coat { get; set; }
        // Размер породы
        public int SizeId { get; set; }
        public Size? Size { get; set; }

        public Guid ExternalId { get; set; } = Guid.NewGuid(); // Для связи с внешними сервисами
    }
}
