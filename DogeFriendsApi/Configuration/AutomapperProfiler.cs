using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;

namespace DogeFriendsApi.Configuration
{
    public class AutomapperProfiler : Profile
    {
        public AutomapperProfiler()
        {
            CreateMap<Coat, CoatDto>();
            CreateMap<CoatDto, Coat>();
        }
    }
}
