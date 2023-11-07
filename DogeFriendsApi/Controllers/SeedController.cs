using Microsoft.AspNetCore.Mvc;
using DogeFriendsApi.Models;
using DogeFriendsApi.Interfaces;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        /// <summary>
        /// Запускает процесс сидирования пород собак.
        /// </summary>
        /// <returns>Результат выполнения сидирования.</returns>
        [HttpPost("SeedBreeds")]
        public async Task<IActionResult> SeedBreeds()
        {
            var result = await _seedService.SeedBreeds();

            return result switch
            {
                RepoAnswer.Success => Ok("Породы успешно сидированы."),
                RepoAnswer.NotFound => NotFound("JSON файл с породами собак не найден."),
                _ => StatusCode(500, "Произошла ошибка во время сидирования пород собак.")
            };
        }
    }
}