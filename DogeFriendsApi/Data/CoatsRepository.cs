using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;


namespace DogeFriendsApi.Data
{
    public class CoatsRepository : ICoatsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoatsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(CoatDto?, RepoAnswer)> GetCoatAsync(int id)
        {
            var result = await _context.Coats.FindAsync(id);
            if (result != null)
            {
                return (_mapper.Map<CoatDto>(result), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(IEnumerable<CoatDto>?, RepoAnswer)> GetAllCoatsAsync()
        {
            var result = await _context.Coats.ToListAsync();
            if (result.Any())
            {
                return (result.Select(coat => _mapper.Map<CoatDto>(coat)), RepoAnswer.Success);
            }
            return (null, RepoAnswer.NotFound);
        }

        public async Task<(CoatDto?, RepoAnswer)> CreateCoatAsync(CoatDto coat)
        {
            var alreadyExist = await _context.Coats.AnyAsync(x => x.Name == coat.Name);
            if (alreadyExist)
            {
                return (null, RepoAnswer.AlreadyExist);
            }

            var newCoat = new Coat() { Name = coat.Name };
            _context.Coats.Add(newCoat);
            await _context.SaveChangesAsync();

            return (_mapper.Map<CoatDto>(newCoat), RepoAnswer.Success);
        }

        public async Task<(CoatDto?, RepoAnswer)> UpdateCoatAsync(int id, CoatDto coat)
        {
            var foundCoat = await _context.Coats.FindAsync(id);
            if (foundCoat == null)
            {
                return (null, RepoAnswer.NotFound);
            }

            var existingCoatWithSameName = await _context.Coats.FirstOrDefaultAsync(c => c.Name == coat.Name && c.Id != id);
            if (existingCoatWithSameName != null)
            {
                return (_mapper.Map<CoatDto>(existingCoatWithSameName), RepoAnswer.AlreadyExist);
            }

            foundCoat.Name = coat.Name;
            await _context.SaveChangesAsync();

            return (_mapper.Map<CoatDto>(foundCoat), RepoAnswer.Success);
        }

        public async Task<RepoAnswer> DeleteCoatAsync(int id)
        {
            var foundCoat = await _context.Coats.FindAsync(id);
            if (foundCoat == null)
            {
                return RepoAnswer.NotFound;
            }

            _context.Coats.Remove(foundCoat);
            await _context.SaveChangesAsync();
            return RepoAnswer.Success;
        }
    }
}
