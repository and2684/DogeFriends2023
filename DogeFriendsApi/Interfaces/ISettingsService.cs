namespace DogeFriendsApi.Interfaces
{
    public interface ISettingsService
    {
        public Task<string?> GetConnectionStringAsync(string key, string encryptionKey);
    }
}
