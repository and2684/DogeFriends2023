using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface ISizesRepository
    {
        Task<(SizeDto?, RepoAnswer)> GetSizeAsync(int id);
        Task<(IEnumerable<SizeDto>?, RepoAnswer)> GetAllSizesAsync();
        Task<(SizeDto?, RepoAnswer)> CreateSizeAsync(SizeDto size);
        Task<(SizeDto?, RepoAnswer)> UpdateSizeAsync(int id, SizeDto size);
        Task<RepoAnswer> DeleteSizeAsync(int id);
    }
}
