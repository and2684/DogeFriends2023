namespace DogeFriendsSharedClassLibrary.Interfaces
{
    public interface ISettingsService
    {
        public Task<string?> GetSettingValue(string key, string encryptionKey);
    }
}
