using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using App.Dto.Response;
using App.Entities.Security;
using Autofac;
using AutoMapper;

namespace App.Services.Mappings.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // create user map 
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => source.UserName))
                .ForMember(destination => destination.EmailAddress, options => options.MapFrom(source => source.Email));
        }
    }
}
