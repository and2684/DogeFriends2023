using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class FriendshipsRepository : IFriendshipsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FriendshipsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(List<UserInfoDto>?, RepoAnswer)> GetFriendsAsync(int userId)
        {
            var friends = await _context.Friendships.Where(x => x.UserId == userId).Select(x => x.Friend).ToListAsync();
            if (friends.Any())
            {
                return (friends.Select(friend => _mapper.Map<UserInfoDto>(friend)).ToList(), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<RepoAnswer> CreateFriendshipAsync(int userId, int friendId)
        {
            var user = await _context.Users.FindAsync(userId);
            var friend = await _context.Users.FindAsync(friendId);
            if (user == null || friend == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Friendships.Add(new Friendship { UserId = userId, FriendId = friendId });
            _context.Friendships.Add(new Friendship { FriendId = userId, UserId = friendId });
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }

        public async Task<RepoAnswer> RemoveFriendshipAsync(int userId, int friendId)
        {
            var friendship1 = await _context.Friendships.Where(x => x.UserId == userId && x.FriendId == friendId).FirstOrDefaultAsync();
            var friendship2 = await _context.Friendships.Where(x => x.UserId == friendId && x.FriendId == userId).FirstOrDefaultAsync();

            if (friendship1 == null || friendship2 == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Friendships.Remove(friendship1);
            _context.Friendships.Remove(friendship2);
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }
    }
}
