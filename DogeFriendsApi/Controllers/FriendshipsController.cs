using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipsRepository _friendshipsRepository;

        public FriendshipsController(IFriendshipsRepository friendshipsRepository)
        {
            _friendshipsRepository = friendshipsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriendships(int userId)
        {
            var (friendships, answerCode) = await _friendshipsRepository.GetFriendsAsync(userId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"No friends found. That's sad.");
                case RepoAnswer.Success:
                    return Ok(friendships);
                default:
                    return StatusCode(500, $"An error occurred while getting friends.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendship(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.CreateFriendshipAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"User id or friend id not found.");
                case RepoAnswer.Success:
                    return Ok("Now u are friends <3");
                default:
                    return StatusCode(500, $"An error occurred while establishing friendship.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFriendship(int userId, int friendId)
        {
            var answerCode = await _friendshipsRepository.RemoveFriendshipAsync(userId, friendId);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"User id or friend id not found.");
                case RepoAnswer.Success:
                    return Ok("Friendship no more. :(");
                default:
                    return StatusCode(500, $"An error occurred while cancelling friendship.");
            }
        }
    }
}
