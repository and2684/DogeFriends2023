using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface IDogRepository
    {
        Task<(DogDto?, RepoAnswer)> GetDogAsync(int id);
        Task<(IEnumerable<DogDto>?, RepoAnswer)> GetAllDogsAsync();
        Task<(DogDto?, RepoAnswer)> CreateDogAsync(DogDto dog);
        Task<(DogDto?, RepoAnswer)> UpdateDogAsync(int id, DogDto dog);
        Task<RepoAnswer> DeleteDogAsync(int id);
    }
}
