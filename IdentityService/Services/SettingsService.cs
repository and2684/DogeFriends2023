﻿using DogeFriendsSharedClassLibrary.Interfaces;

namespace IdentityService.Services
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

        public async Task<string?> GetSettingValue(string key, string encryptionKey)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear(); // Каждое обращение к сервису должно быть отдельным
                _httpClient.DefaultRequestHeaders.Add("Key", key);
                _httpClient.DefaultRequestHeaders.Add("EncryptionKey", encryptionKey);
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
