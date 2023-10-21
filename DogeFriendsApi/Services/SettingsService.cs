using DogeFriendsApi.Interfaces;

namespace DogeFriendsApi.Services
{
    public class SettingsService : ISettingsService
    {
        public string SettingsServiceUrl { get; set; }
        private readonly HttpClient _httpClient;

        public SettingsService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            SettingsServiceUrl = config.GetSection("SettingsService:SettingsServiceUrl").Value!;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri($"{SettingsServiceUrl}/api/settings/");
        }

        public async Task<string?> GetConnectionStringAsync(string key, string encryptionKey)
        {
            _httpClient.DefaultRequestHeaders.Add("Key", key);
            _httpClient.DefaultRequestHeaders.Add("EncryptionKey", encryptionKey);
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            // Обработка ошибки, например, вывод сообщения об ошибке
            Console.WriteLine($"Error: {response.StatusCode}");
            return null;
        }
    }
}
