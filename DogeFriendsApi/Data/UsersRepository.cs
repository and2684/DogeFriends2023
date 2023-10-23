using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<(UserInfoDto?, RepoAnswer)> UpdateUserAsync(User user)
        {
            var foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var emailAlreadyTaken = await _context.Users.AnyAsync(x => x.Username.ToLower() == user.Username.ToLower());
            if (emailAlreadyTaken)
            {
                return (null, RepoAnswer.EmailTaken);
            }

            foundUser.FirstName = user.FirstName;
            foundUser.LastName = user.LastName;
            foundUser.Email = user.Email;
            foundUser.Description = user.Description;
            foundUser.Hometown = user.Hometown;
            await _context.SaveChangesAsync();

            return (_mapper.Map<UserInfoDto>(foundUser), RepoAnswer.Success);
        }
    }
}
