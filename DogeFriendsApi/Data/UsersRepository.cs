using AutoMapper;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DogeFriendsApi.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public UsersRepository(DataContext context, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _mapper = mapper;
            _client = httpClientFactory.CreateClient("RegisterClient");

            _client.BaseAddress = new Uri("https://localhost:5101"); // URL Identity server (ХРАНИМ В SETTINGS SERVICE)
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(IEnumerable<UserInfoDto>?, RepoAnswer)> GetAllUsersAsync()
        {
            var result = await _context.Users.ToListAsync();
            if (result.Any())
            {
                return (result.Select(user => _mapper.Map<UserInfoDto>(user)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(UserInfoDto?, RepoAnswer)> GetUserByIdAsync(int id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result != null)
            {
                return (_mapper.Map<UserInfoDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(UserInfoDto?, RepoAnswer)> GetUserByUsernameAsync(string username)
        {
            var result = await _context.Users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (result != null)
            {
                return (_mapper.Map<UserInfoDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(UserInfoDto?, RepoAnswer)> UpdateUserAsync(UserInfoDto user)
        {
            var foundUser = await _context.Users.Where(x => x.Username.ToLower() == user.Username.ToLower()).FirstOrDefaultAsync();
            if (foundUser == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var emailAlreadyTaken = await _context.Users.AnyAsync(x => x.Email.ToLower() == user.Email.ToLower() && x.Username.ToLower() != user.Username.ToLower());
            if (emailAlreadyTaken)
            {
                return (null, RepoAnswer.EmailTaken);
            }

            if ((new System.Net.Mail.MailAddress(user.Email)).Address != user.Email)
            {
                return (null, RepoAnswer.ActionFailed);
            }

            foundUser.FirstName = user.FirstName;
            foundUser.LastName = user.LastName;
            foundUser.Email = user.Email.ToLower();
            foundUser.Description = user.Description;
            foundUser.Hometown = user.Hometown;
            await _context.SaveChangesAsync();

            return (_mapper.Map<UserInfoDto>(foundUser), RepoAnswer.Success);
        }

        public async Task<(UserLoginResponseDto?, RepoAnswer)> LoginUserAsync(LoginDto user)
        {
            // Формирование данных для отправки
            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );

            // Отправка POST запроса на эндпоинт регистрации пользователя
            var response = await _client.PostAsync("api/identity/register", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            // Десериализация JSON в объект типа UserInfoDto
            var userInfo = JsonSerializer.Deserialize<UserLoginResponseDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response.IsSuccessStatusCode)
                return (userInfo, RepoAnswer.Success);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return (null, RepoAnswer.NotFound);

            // Обработка неудачного ответа
            return (userInfo, RepoAnswer.ActionFailed);
        }

        public async Task<(UserLoginResponseDto?, RepoAnswer)> RegisterUserAsync(RegisterDto user)
        {
            // Формирование данных для отправки
            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );

            // Отправка POST запроса на эндпоинт регистрации пользователя
            var response = await _client.PostAsync("api/identity/register", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            // Десериализация JSON в объект типа UserInfoDto
            var userInfo = JsonSerializer.Deserialize<UserLoginResponseDto>(responseContent, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (response.IsSuccessStatusCode)
                return (userInfo, RepoAnswer.Success);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return (null, RepoAnswer.NotFound);

            // Обработка неудачного ответа
            return (userInfo, RepoAnswer.ActionFailed);
        }
    }
}
