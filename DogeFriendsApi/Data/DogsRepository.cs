using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class DogsRepository : IDogsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DogsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<DogDto>?, RepoAnswer)> GetAllDogsAsync()
        {
            var result = await _context.Dogs
                .Include(b => b.User)
                .Include(b => b.Breed)
                .ToListAsync();

            if (result.Any())
            {
                return (result.Select(dog => _mapper.Map<DogDto>(dog)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(IEnumerable<DogDto>?, RepoAnswer)> GetDogsByUsernameAsync(string username)
        {
            var result = await _context.Dogs
                .Include(b => b.User)
                .Include(b => b.Breed)
                .Where(x => x.User!.Username == username)
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (result.Any())
            {
                return (result.Select(dog => _mapper.Map<DogDto>(dog)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(DogDto?, RepoAnswer)> GetDogAsync(int id)
        {
            //var result = await _context.Dogs
            //    .Include(b => b.User)
            //    .Include(b => b.Breed)
            //    .FirstOrDefaultAsync(b => b.Id == id);

            var result = await _context.Dogs.FirstOrDefaultAsync(b => b.Id == id);
            if (result != null)
            {
                await _context.Entry(result).Reference(b => b.User).LoadAsync();
                await _context.Entry(result).Reference(b => b.Breed).LoadAsync();
            }

            if (result != null)
            {
                return (_mapper.Map<DogDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(DogDto?, RepoAnswer)> CreateDogAsync(DogAddOrUpdateDto dog)
        {
            var alreadyExist = await _context.Dogs.AnyAsync(x => x.Name == dog.Name && x.UserId == dog.UserId);
            if (alreadyExist)
            {
                return (null, RepoAnswer.AlreadyExist);
            }

            var newDog = new Dog()
            {
                Name = dog.Name,
                BreedId = _context.Breeds.Where(x => x.Id == dog.BreedId).Select(x => x.Id).FirstOrDefault(),
                Breed = _context.Breeds.FirstOrDefault(x => x.Id == dog.BreedId),
                UserId = _context.Users.Where(x => x.Id == dog.UserId).Select(x => x.Id).FirstOrDefault(),
                User = _context.Users.FirstOrDefault(x => x.Id == dog.UserId)
            };

            _context.Dogs.Add(newDog);
            await _context.SaveChangesAsync();

            return (_mapper.Map<DogDto>(newDog), RepoAnswer.Success);
        }

        public async Task<(DogDto?, RepoAnswer)> UpdateDogAsync(int id, DogAddOrUpdateDto dog)
        {
            var foundDog = await _context.Dogs.FindAsync(id);
            if (foundDog == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var existingDog = await _context.Dogs.FirstOrDefaultAsync(c => c.Name == dog.Name && c.Id != id);
            if (existingDog != null)
            {
                return (_mapper.Map<DogDto>(existingDog), RepoAnswer.AlreadyExist);
            }

            foundDog.Name = dog.Name;
            foundDog.BreedId = _context.Breeds.Where(x => x.Id == dog.BreedId).Select(x => x.Id).FirstOrDefault();
            foundDog.UserId = _context.Sizes.Where(x => x.Id == dog.UserId).Select(x => x.Id).FirstOrDefault();

            await _context.SaveChangesAsync();

            return (_mapper.Map<DogDto>(foundDog), RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteDogAsync(int id)
        {
            var foundDog = await _context.Dogs.FindAsync(id);
            if (foundDog == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Dogs.Remove(foundDog);
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }
    }
}
