using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedsRepository _breedsRepository;

        public BreedsController(IBreedsRepository breedsRepository)
        {
            _breedsRepository = breedsRepository;
        }

        // GET: api/<BreedsController>
        [HttpGet]
        public async Task<IActionResult> GetAllBreeds()
        {
            var (breeds, answerCode) = await _breedsRepository.GetAllBreedsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("No breeds found.");
                case RepoAnswer.Success:
                    return Ok(breeds);
                default:
                    return StatusCode(500, "An error occurred while retrieving the breeds.");
            }
        }

        // GET api/<BreedsController>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBreed(int id)
        {
            var (breed, answerCode) = await _breedsRepository.GetBreedAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Breed with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok(breed);
                default:
                    return StatusCode(500, $"An error occurred while retrieving the breed with id = {id}.");
            }
        }

        // POST api/<BreedsController>
        [HttpPost]
        public async Task<IActionResult> CreateBreed([FromBody] BreedDto breed)
        {
            var (newBreed, answerCode) = await _breedsRepository.CreateBreedAsync(breed);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Breed already exists.");
                case RepoAnswer.Success:
                    return Ok(newBreed);
                default:
                    return StatusCode(500, "An error occurred while creating breed.");
            }
        }

        // PUT api/<BreedsController>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBreed(int id, [FromBody] BreedDto breed)
        {
            var (updatedBreed, answerCode) = await _breedsRepository.UpdateBreedAsync(id, breed);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Breed with name {breed.Name} already exists.");
                case RepoAnswer.NotFound:
                    return NotFound($"Breed with id = {id} not found");
                case RepoAnswer.Success:
                    return Ok(updatedBreed);
                default:
                    return StatusCode(500, "An error occurred while creating breed.");
            }
        }

        // DELETE api/<BreedsController>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBreed(int id)
        {
            var answerCode = await _breedsRepository.DeleteBreedAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Breed with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"An error occurred while retrieving the breed with id = {id}.");
            }
        }
    }
}
