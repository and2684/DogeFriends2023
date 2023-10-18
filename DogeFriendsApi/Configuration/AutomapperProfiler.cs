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

            CreateMap<Size, SizeDto>();
            CreateMap<SizeDto, Size>();

            CreateMap<User, UserInfoDto>()
                .ForMember(dest => dest.Showname, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
