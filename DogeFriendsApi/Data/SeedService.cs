using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Newtonsoft.Json;

namespace DogeFriendsApi.Data
{
    public class SeedService : ISeedService
    {
        private readonly DataContext _context;

        public SeedService(DataContext context)
        {
            _context = context;
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

                    Random random = new Random();

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
                }

                await _context.SaveChangesAsync(); // Сохранение изменений
                return RepoAnswer.Success;
            }
            return RepoAnswer.NotFound;
        }
    }
}
