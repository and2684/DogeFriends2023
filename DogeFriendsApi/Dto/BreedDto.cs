using System.ComponentModel.DataAnnotations;

namespace DogeFriendsApi.Dto
{
    public class BreedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CoatId { get; set; }
        public int SizeId { get; set; }
    }
}
