using AutoMapper;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;
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
            var subscribers = await _context.Friendships.Where(x => x.FriendId == userId).Select(x => x.User).ToListAsync();
            var subscriptions = await _context.Friendships.Where(x => x.UserId == userId).Select(x => x.Friend).ToListAsync();

            if (subscribers != null && subscriptions != null)
            {
                var friends = subscribers.Intersect(subscriptions);

                if (friends.Any())
                {
                    return (friends.Select(_mapper.Map<UserInfoDto>).ToList(), RepoAnswer.Success);
                }
            }

            return (null, RepoAnswer.NotFound);
        }
        public async Task<(List<UserInfoDto>?, RepoAnswer)> GetSubscribersAsync(int userId)
        {
            var subscribers = await _context.Friendships.Where(x => x.FriendId == userId).Select(x => x.User).ToListAsync();
            var subscriptions = await _context.Friendships.Where(x => x.UserId == userId).Select(x => x.Friend).ToListAsync();

            if (subscribers != null)
            {
                var result = subscribers.Except(subscriptions);
                if (result.Any())
                {
                    return (result.Select(_mapper.Map<UserInfoDto>).ToList(), RepoAnswer.Success);
                }
            }

            return (null, RepoAnswer.NotFound);
        }

        public async Task<(List<UserInfoDto>?, RepoAnswer)> GetSubscribtionsAsync(int userId)
        {
            var subscribers = await _context.Friendships.Where(x => x.FriendId == userId).Select(x => x.User).ToListAsync();
            var subscriptions = await _context.Friendships.Where(x => x.UserId == userId).Select(x => x.Friend).ToListAsync();

            if (subscribers != null)
            {
                var result = subscriptions.Except(subscribers);
                if (result.Any())
                {
                    return (result.Select(_mapper.Map<UserInfoDto>).ToList(), RepoAnswer.Success);
                }
            }

            return (null, RepoAnswer.NotFound);
        }

        public async Task<RepoAnswer> SubscribeAsync(int userId, int friendId)
        {
            if (userId == friendId)
            {
                return RepoAnswer.ActionFailed; // Нельзя подписаться на себя
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == friendId);

            if (user == null || friend == null)
            {
                return RepoAnswer.NotFound;
            }

            var existingSubscription = await _context.Friendships
                .AnyAsync(x => (x.UserId == userId && x.FriendId == friendId));
            if (existingSubscription)
            {
                return RepoAnswer.AlreadyExist;
            }

            _context.Friendships.Add(new Friendship { UserId = userId, FriendId = friendId });

            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }

        public async Task<RepoAnswer> UnsubscribeAsync(int userId, int friendId)
        {            
            var subscription = await _context.Friendships.Where(x => x.UserId == userId && x.FriendId == friendId).FirstOrDefaultAsync();

            if (subscription == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Remove(subscription);

            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }

    }
}
