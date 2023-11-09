using DogeFriendsApi.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using DogeFriendsSharedClassLibrary.Dto;

namespace DogeFriendsApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _client;

        public TokenService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _client = httpClientFactory.CreateClient("Token validation and refresh client");
            _client.BaseAddress = new Uri(config["IdentityService:IdentityServiceUrl"]!); // URL Identity server
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> ValidateAccessTokenAsync(string accessToken)
        {
            var jsonRequestData = JsonConvert.SerializeObject(accessToken);
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/identity/validatetoken", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokensAsync(TokensDto tokensDto)
        {
            var jsonRequestData = JsonConvert.SerializeObject(tokensDto);
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/identity/refreshtoken", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokens = JsonConvert.DeserializeObject<(string accessToken, string refreshToken)>(result);
                return tokens;
            }

            throw new Exception("Ошибка при проверке валидности токена.");
        }
    }
}