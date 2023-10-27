using DogeFriendsApi.Interfaces;

namespace DogeFriendsApi.Services
{
    public class SettingsService : ISettingsService
    {
        public string SettingsServiceUrl { get; set; }
        private readonly HttpClient _httpClient;
        private readonly ILoggerManager _logger;

        public SettingsService(IConfiguration config, IHttpClientFactory httpClientFactory, ILoggerManager logger)
        {
            SettingsServiceUrl = config.GetSection("SettingsService:SettingsServiceUrl").Value!;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri($"{SettingsServiceUrl}/api/settings/");
            _logger = logger;
        }

        public async Task<string?> GetConnectionStringAsync(string key, string encryptionKey)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Key", key);
                _httpClient.DefaultRequestHeaders.Add("EncryptionKey", encryptionKey);
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
