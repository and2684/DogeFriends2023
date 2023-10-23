namespace DogeFriendsApi.Dto
{
    public class DogDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BreedId { get; set; }
        public string DogBreed { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string DogUser { get; set; } = string.Empty;
    }
}
