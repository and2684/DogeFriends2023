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

            switch (result)
            {
                case RepoAnswer.Success:
                    return Ok("Породы успешно сидированы.");
                case RepoAnswer.NotFound:
                    return NotFound("JSON файл с породами собак не найден.");
                default:
                    return StatusCode(500, "Произошла ошибка во время сидирования пород собак.");
            }
        }
    }
}