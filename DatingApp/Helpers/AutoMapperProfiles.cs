using AutoMapper;
using DatingApp.Core;
using DatingApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<User, UserForDetailedDto>().ForMember(dest => dest.PhotoUrl, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).UrlPhoto))
                .ForMember(dest => dest.Age, options => options.MapFrom(src => src.DateBirth.CalculateAge()));
            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).UrlPhoto))
                .ForMember(dest => dest.Age, options => options.MapFrom(src => src.DateBirth.CalculateAge()));
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
