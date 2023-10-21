using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpPost("SeedBreeds")]
        public async Task<IActionResult> SeedBreeds()
        {
            var result = await _seedService.SeedBreeds();

            switch (result)
            {
                case RepoAnswer.Success:
                    return Ok("Breeds seeded successfully.");
                case RepoAnswer.NotFound:
                    return NotFound("Breeds JSON file not found.");
                default:
                    return StatusCode(500, "An error occurred while seeding breeds.");
            }
        }
    }
}
