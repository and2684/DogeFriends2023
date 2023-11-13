namespace DogeFriendsApi.Dto
{
    public class DogAddOrUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public int BreedId { get; set; }
        public int UserId { get; set; }
    }
}
