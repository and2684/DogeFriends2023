namespace DogeFriendsApi.Interfaces
{
    public interface ISettingsService
    {
        public Task<string?> GetSettingValue(string key, string encryptionKey);
    }
}
