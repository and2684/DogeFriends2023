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

            CreateMap<Breed, BreedDto>()
                .ForMember(dest => dest.BreedGroups, opt => opt.MapFrom(src => string.Join(", ", src.BreedGroups!.Select(bg => bg.Name))))
                .ForMember(dest => dest.Coat, opt => opt.MapFrom(src => src.Coat!.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size!.Name));
            CreateMap<BreedDto, Breed>();
        }
    }
}