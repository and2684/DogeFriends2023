namespace DogeFriendsApi.Interfaces
{
    public interface IImageService
    {
        public Task<string> GetMainImage64(string uid, string entityName);
    }
}
