using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var (users, answerCode) = await _usersRepository.GetAllUsersAsync();
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound("No users found.");
                case RepoAnswer.Success:
                    return Ok(users);
                default:
                    return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var (user, answerCode) = await _usersRepository.GetUserByIdAsync(id);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"User with id = {id} not found.");
                case RepoAnswer.Success:
                    return Ok(user);
                default:
                    return StatusCode(500, $"An error occurred while retrieving user with id = {id}.");
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var (user, answerCode) = await _usersRepository.GetUserByUsernameAsync(username);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Username {username} not found.");
                case RepoAnswer.Success:
                    return Ok(user);
                default:
                    return StatusCode(500, $"An error occurred while retrieving user {username}.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var (updatedUser, answerCode) = await _usersRepository.UpdateUserAsync(user);
            switch (answerCode)
            {
                case RepoAnswer.NotFound:
                    return NotFound($"Username {user.Username} not found.");
                case RepoAnswer.EmailTaken:
                    return Conflict($"Email {user.Email} already taken.");
                case RepoAnswer.Success:
                    return Ok(updatedUser);
                default:
                    return StatusCode(500, $"An error occurred while updating user {user.Username}.");
            }
        }
    }
}
