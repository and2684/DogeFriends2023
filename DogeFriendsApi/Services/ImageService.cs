using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using ImageService.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DogeFriendsApi.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _client;

        public ImageService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ImageClient");

            _client.BaseAddress = new Uri(config["ImageService:ImageServiceUrl"]!); // URL ImageService
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetMainImage64(string uid, string entityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/images/getmain");
            request.Headers.Add("UID", uid);
            request.Headers.Add("EntityName", entityName);

            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var mainImage = JsonConvert.DeserializeObject<ImageModel>(json);
                if (mainImage != null)
                    return mainImage.Base64Data;
            }
            return string.Empty;
        }
    }
}
