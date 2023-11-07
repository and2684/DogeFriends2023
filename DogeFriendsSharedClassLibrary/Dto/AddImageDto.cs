namespace DogeFriendsSharedClassLibrary.Dto
{
    public class AddImageDto
    {
        public string UID { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string Base64Data { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
