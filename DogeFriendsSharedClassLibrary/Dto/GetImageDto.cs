using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsSharedClassLibrary.Dto
{
    public class GetImageDto
    {
        [FromHeader]
        public string UID { get; set; } = string.Empty;
        [FromHeader]
        public string EntityName { get; set; } = string.Empty;
    }
}
