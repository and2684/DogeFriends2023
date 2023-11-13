using DogeFriendsApi.Data;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace DogeFriendsApi.Services
{
    public class SeedService : ISeedService
    {
        private readonly DataContext _context;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public SeedService(DataContext context, IHttpClientFactory httpClientFactory, ILoggerManager logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _httpClient = httpClientFactory.CreateClient("ImageClient");
        }

        public async Task<RepoAnswer> SeedBreeds()
        {
            string jsonFilePath = Path.Combine("Seed", "Breeds.json"); // Оставлю литералом, не потащу в конфиг

            if (File.Exists(jsonFilePath))
            {
                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                List<BreedDto> breedsFromJsonList = JsonConvert.DeserializeObject<List<BreedDto>>(jsonContent)!;

                if (breedsFromJsonList.Count == 0) return RepoAnswer.NotFound;

                foreach (var breedDto in breedsFromJsonList)
                {
                    var breedGroupStringList = breedDto.BreedGroups.Split(',').Select(x => x.Trim()).ToList();

                    var random = new Random();

                    var breed = new Breed
                    {
                        Name = breedDto.Name,
                        Description = breedDto.Description,
                        CoatId = random.Next(1, 4), // Случайное значение
                        SizeId = random.Next(1, 4),  // Случайное значение
                        BreedGroups = _context.BreedGroups.Where(x => breedGroupStringList.Contains(x.Name)).ToList()
                    };

                    // Сохраним породу в базе данных
                    _context.Breeds.Add(breed);

                    // Добавим фотку в базу данных ImageService
                    if (breedDto.MainImage.IsNullOrEmpty()) continue;

                    // Отправка запроса к ImageService для добавления изображения
                    var imageRequest = new AddImageDto
                    {
                        UID = breed.ExternalId.ToString(),
                        EntityName = "Breed",
                        Base64Data = breedDto.MainImage,
                        IsMain = true
                    };

                    var json = JsonConvert.SerializeObject(imageRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var imageUrl = _config["ImageService:ImageServiceUrl"];

                    var response = await _httpClient.PostAsync($"{imageUrl}/api/images/add", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarn($"Не удалось сохранить картинку породы. Порода - {breed.Name}. Код ответа - {response.StatusCode}.");
                    }
                }

                await _context.SaveChangesAsync(); // Сохранение изменений
                return RepoAnswer.Success;
            }
            return RepoAnswer.NotFound;
        }
    }
}
