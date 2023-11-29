namespace DogeFriendsApi.Dto
{
    public class DogDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DogBreedId { get; set; }
        public string DogBreed { get; set; } = string.Empty;
        public string DogUser { get; set; } = string.Empty;
        public string DogUsername { get; set; } = string.Empty;
        public string Base64Image { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
    }
}
