using DogeFriendsApi.Interfaces;

namespace DogeFriendsApi.Services
{
    public class SettingsService : ISettingsService
    {
        public string SettingsServiceUrl { get; set; }
        private readonly HttpClient _httpClient;
        public SettingsService(IConfiguration config)
        {
            SettingsServiceUrl = config.GetSection("SettingsService").GetValue<string>("SettingsServiceUrl")!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{SettingsServiceUrl}/api/settings/")
            };
        }

        public async Task<string?> GetConnectionStringAsync(string key)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(key);

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
