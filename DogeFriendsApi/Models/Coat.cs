namespace DogeFriendsApi.Models
{
    public class Coat
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Breed>? Breeds { get; set; }
    }
}
