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
                .ForMember(dest => dest.PhotoUrlAvatar, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).UrlPhotoAvatar))
                .ForMember(dest => dest.Age, options => options.MapFrom(src => src.DateBirth.CalculateAge()));
            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).UrlPhoto))
                .ForMember(dest => dest.PhotoUrlAvatar, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).UrlPhotoAvatar))
                .ForMember(dest => dest.Age, options => options.MapFrom(src => src.DateBirth.CalculateAge()));
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageForReturnDto>()
                .ForMember(dest => dest.SenderUrlPhotoAvatar, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).UrlPhotoAvatar))
                .ForMember(dest => dest.RecipientUrlPhotoAvatar, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).UrlPhotoAvatar));
        }
    }
}
