using AutoMapper;
using DogeFriendsApi.Dto;
using DogeFriendsApi.Models;
using DogeFriendsSharedClassLibrary.Dto;

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

            CreateMap<User, RegisterDto>();
            CreateMap<RegisterDto, User>();

            CreateMap<Breed, BreedDto>()
                .ForMember(dest => dest.BreedGroups, opt => opt.MapFrom(src => string.Join(", ", src.BreedGroups!.Select(bg => bg.Name))))
                .ForMember(dest => dest.Coat, opt => opt.MapFrom(src => src.Coat!.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size!.Name));
            CreateMap<BreedDto, Breed>();

            CreateMap<Dog, DogDto>()
                .ForMember(dest => dest.DogBreedId, opt => opt.MapFrom(src => src.Breed!.Id))
                .ForMember(dest => dest.DogBreed, opt => opt.MapFrom(src => src.Breed!.Name))
                .ForMember(dest => dest.DogUser, opt => opt.MapFrom(src => $"{src.User!.FirstName} {src.User!.LastName}"))
                .ForMember(dest => dest.DogUsername, opt => opt.MapFrom(src => src.User!.Username));
            CreateMap<DogDto, Dog>();

            CreateMap<BreedGroup, BreedGroupDto>();
            CreateMap<BreedGroupDto, BreedGroup>();
        }
    }
}