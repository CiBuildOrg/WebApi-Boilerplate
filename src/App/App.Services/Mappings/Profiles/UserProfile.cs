using System.Collections.Generic;
using App.Dto.Response;
using App.Entities;
using App.Entities.Security;
using AutoMapper;

namespace App.Services.Mappings.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile(IMemberValueResolver<object, object, ICollection<Image>, string> custom)
        {
            // create user map 
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => source.UserName))
                .ForMember(destination => destination.EmailAddress, options => options.MapFrom(source => source.Email))
                .ForMember(destination => destination.AvatarUrl,
                    options => options.ResolveUsing(custom, x => x.ProfileInfo.ProfileImages));
        }
    }
}
