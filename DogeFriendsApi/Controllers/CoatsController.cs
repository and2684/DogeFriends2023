using DogeFriendsApi.Data;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoatsController : ControllerBase
    {
        private readonly ICoatsRepository _coatsRepository;

        public CoatsController(ICoatsRepository coatsRepository)
        {
            _coatsRepository = coatsRepository;
        }

        // GET: api/<CoatsController>
        [HttpGet]
        public async Task<IActionResult> GetAllCoats()
        {
            var (coats, answerCode) = await _coatsRepository.GetAllCoatsAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("No coats found.");
                case RepoAnswer.Success:
                    return Ok(coats);
                default:
                    return StatusCode(500, "An error occurred while retrieving the coats.");
            }
        }

        // GET api/<CoatsController>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCoat(int id)
        {
            var (coat, answerCode) = await _coatsRepository.GetCoatAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Coat with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok(coat);
                default:
                    return StatusCode(500, $"An error occurred while retrieving the coat with id = {id}.");
            }
        }

        // POST api/<CoatsController>
        [HttpPost]
        public async Task<IActionResult> CreateCoat([FromBody] CoatDto coat)
        {
            var (newCoat, answerCode) = await _coatsRepository.CreateCoatAsync(coat);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Coat already exists.");
                case RepoAnswer.Success:
                    return Ok(newCoat);
                default:
                    return StatusCode(500, "An error occurred while creating coat.");
            }
        }

        // PUT api/<CoatsController>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCoat(int id, [FromBody] CoatDto coat)
        {
            var (updatedCoat, answerCode) = await _coatsRepository.UpdateCoatAsync(id, coat);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Coat with name {coat.Name} already exists.");
                case RepoAnswer.NotFound:
                    return NotFound($"Coat with id = {id} not found");
                case RepoAnswer.Success:
                    return Ok(updatedCoat);
                default:
                    return StatusCode(500, "An error occurred while creating coat.");
            }
        }

        // DELETE api/<CoatsController>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCoat(int id)
        {
            var (deleted, answerCode) = await _coatsRepository.DeleteCoatAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Coat with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"An error occurred while retrieving the coat with id = {id}.");
            }
        }
    }
}
