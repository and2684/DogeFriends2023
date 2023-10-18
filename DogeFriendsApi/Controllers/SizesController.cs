using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizesRepository _sizesRepository;

        public SizesController(ISizesRepository sizesRepository)
        {
            _sizesRepository = sizesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizes()
        {
            var (sizes, answerCode) = await _sizesRepository.GetAllSizesAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("No sizes found.");
                case RepoAnswer.Success:
                    return Ok(sizes);
                default:
                    return StatusCode(500, "An error occurred while retrieving the sizes.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSize(int id)
        {
            var (size, answerCode) = await _sizesRepository.GetSizeAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Size with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok(size);
                default:
                    return StatusCode(500, $"An error occurred while retrieving the size with id = {id}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] SizeDto size)
        {
            var (newSize, answerCode) = await _sizesRepository.CreateSizeAsync(size);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict("Size already exists.");
                case RepoAnswer.Success:
                    return Ok(newSize);
                default:
                    return StatusCode(500, "An error occurred while creating size.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] SizeDto size)
        {
            var (updatedSize, answerCode) = await _sizesRepository.UpdateSizeAsync(id, size);
            switch (answerCode)
            {
                case RepoAnswer.AlreadyExist:
                    return Conflict($"Size with name {size.Name} already exists.");
                case RepoAnswer.NotFound:
                    return NotFound($"Size with id = {id} not found");
                case RepoAnswer.Success:
                    return Ok(updatedSize);
                default:
                    return StatusCode(500, "An error occurred while creating size.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var answerCode = await _sizesRepository.DeleteSizeAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Size with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok();
                default:
                    return StatusCode(500, $"An error occurred while retrieving the size with id = {id}.");
            }
        }
    }
}