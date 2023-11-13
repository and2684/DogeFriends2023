using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface IDogsRepository
    {
        Task<(DogDto?, RepoAnswer)> GetDogAsync(int id);
        Task<(IEnumerable<DogDto>?, RepoAnswer)> GetAllDogsAsync();
        Task<(DogDto?, RepoAnswer)> CreateDogAsync(DogAddOrUpdateDto dog);
        Task<(DogDto?, RepoAnswer)> UpdateDogAsync(int id, DogAddOrUpdateDto dog);
        Task<RepoAnswer> DeleteDogAsync(int id);
    }
}
