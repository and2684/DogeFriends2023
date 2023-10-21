using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface ISeedService
    {
        Task<RepoAnswer> SeedBreeds();
    }
}
