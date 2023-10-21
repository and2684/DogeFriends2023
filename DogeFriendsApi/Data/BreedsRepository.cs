﻿using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class BreedsRepository : IBreedsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BreedsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BreedDto>?, RepoAnswer)> GetAllBreedsAsync()
        {
            var result = await _context.Breeds.ToListAsync();
            if (result.Any())
            {
                return (result.Select(breed => _mapper.Map<BreedDto>(breed)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(BreedDto?, RepoAnswer)> GetBreedAsync(int id)
        {
            var result = await _context.Breeds.FindAsync(id);
            if (result != null)
            {
                return (_mapper.Map<BreedDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(BreedDto?, RepoAnswer)> CreateBreedAsync(BreedDto breed)
        {
            var alreadyExist = await _context.Breeds.AnyAsync(x => x.Name == breed.Name);
            if (alreadyExist)
            {
                return (null, RepoAnswer.AlreadyExist);
            }

            var newBreed = new Breed() 
            { 
                Name = breed.Name, 
                Description = breed.Description,  
                CoatId = breed.CoatId,
                SizeId = breed.SizeId
            };

            _context.Breeds.Add(newBreed);
            await _context.SaveChangesAsync();

            return (_mapper.Map<BreedDto>(newBreed), RepoAnswer.Success);
        }

        public async Task<(BreedDto?, RepoAnswer)> UpdateBreedAsync(int id, BreedDto breed)
        {
            var foundBreed = await _context.Breeds.FindAsync(id);
            if (foundBreed == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var existingBreedWithSameName = await _context.Breeds.FirstOrDefaultAsync(c => c.Name == breed.Name && c.Id != id);
            if (existingBreedWithSameName != null)
            {
                return (_mapper.Map<BreedDto>(existingBreedWithSameName), RepoAnswer.AlreadyExist);
            }

            foundBreed.Name = breed.Name;
            foundBreed.Description = breed.Description;
            foundBreed.CoatId = breed.CoatId;
            foundBreed.SizeId = breed.SizeId;

            await _context.SaveChangesAsync();

            return (_mapper.Map<BreedDto>(foundBreed), RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteBreedAsync(int id)
        {
            var foundBreed = await _context.Breeds.FindAsync(id);
            if (foundBreed == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Breeds.Remove(foundBreed);
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }
    }
}
