namespace DogeFriendsApi.Dto
{
    public class BreedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Coat { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string BreedGroups { get; set; } = string.Empty;
    }
}
