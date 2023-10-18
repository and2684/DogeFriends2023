using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface ICoatsRepository
    {
        Task<(CoatDto?, RepoAnswer)> GetCoatAsync(int id);
        Task<(IEnumerable<CoatDto>?, RepoAnswer)> GetAllCoatsAsync();
        Task<(CoatDto?, RepoAnswer)> CreateCoatAsync(CoatDto coat);
        Task<(CoatDto?, RepoAnswer)> UpdateCoatAsync(int id, CoatDto coat);
        Task<RepoAnswer> DeleteCoatAsync(int id);
    }
}
