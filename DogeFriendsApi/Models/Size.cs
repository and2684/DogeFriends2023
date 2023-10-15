namespace DogeFriendsApi.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Breed>? Breeds { get; set; }
    }
}
