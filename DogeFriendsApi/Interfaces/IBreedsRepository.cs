﻿using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Interfaces
{
    public interface IBreedsRepository
    {
        Task<(BreedDto?, RepoAnswer)> GetBreedAsync(int id);
        Task<(IEnumerable<BreedDto>?, RepoAnswer)> GetAllBreedsAsync();
        Task<(BreedDto?, RepoAnswer)> CreateBreedAsync(BreedDto breed);
        Task<(BreedDto?, RepoAnswer)> UpdateBreedAsync(int id, BreedDto breed);
        Task<RepoAnswer> DeleteBreedAsync(int id);
        Task<RepoAnswer> SeedBreedsFromFile(string file);
    }
}