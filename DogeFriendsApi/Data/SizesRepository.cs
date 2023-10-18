using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class SizesRepository : ISizesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SizesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(SizeDto?, RepoAnswer)> GetSizeAsync(int id)
        {
            var result = await _context.Sizes.FindAsync(id);
            if (result != null)
            {
                return (_mapper.Map<SizeDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(IEnumerable<SizeDto>?, RepoAnswer)> GetAllSizesAsync()
        {
            var result = await _context.Sizes.ToListAsync();
            if (result.Any())
            {
                return (result.Select(size => _mapper.Map<SizeDto>(size)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }


        public async Task<(SizeDto?, RepoAnswer)> CreateSizeAsync(SizeDto size)
        {
            var alreadyExist = await _context.Sizes.AnyAsync(x => x.Name.ToLower() == size.Name.ToLower());
            if (alreadyExist)
            {
                return (null, RepoAnswer.AlreadyExist);
            }

            var newSize = new Size() { Name = size.Name };
            _context.Sizes.Add(newSize);
            await _context.SaveChangesAsync();

            return (_mapper.Map<SizeDto>(newSize), RepoAnswer.Success);
        }

        public async Task<(SizeDto?, RepoAnswer)> UpdateSizeAsync(int id, SizeDto size)
        {
            var foundSize = await _context.Sizes.FindAsync(id);
            if (foundSize == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var existingSizeWithSameName = await _context.Sizes.FirstOrDefaultAsync(c => c.Name.ToLower() == size.Name.ToLower() && c.Id != id);
            if (existingSizeWithSameName != null)
            {
                return (_mapper.Map<SizeDto>(existingSizeWithSameName), RepoAnswer.AlreadyExist);
            }

            foundSize.Name = size.Name;
            await _context.SaveChangesAsync();

            return (_mapper.Map<SizeDto>(foundSize), RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteSizeAsync(int id)
        {
            var foundSize = await _context.Sizes.FindAsync(id);
            if (foundSize == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Sizes.Remove(foundSize);
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }
    }
}
