using System;
using System.Net.Http;
using System.Web.Http.Routing;
using App.Database.Security;
using App.Dto.Response;
using App.Entities.Security;
using Microsoft.AspNet.Identity;

namespace App.Api.Models
{
    public class ModelFactory
    {
        private readonly UserManager<ApplicationUser, Guid> _appUserManager;

        public ModelFactory(UserManager<ApplicationUser, Guid> userManager)
        {
            _appUserManager = userManager;
        }

        public UserReturnModel Create(ApplicationUser user, HttpRequestMessage request)
        {
            var urlHelper = new UrlHelper(request);
            return new UserReturnModel
            {
                Url = urlHelper.Link("GetUserById", new { id = user.Id }),
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.ProfileInfo.FullName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                JoinDate = user.ProfileInfo.JoinDate,
                Description = user.ProfileInfo.Description,
                Roles = _appUserManager.GetRolesAsync(user.Id).Result,
                Claims = _appUserManager.GetClaimsAsync(user.Id).Result
            };
        }

        public RoleReturnModel Create(CustomRole role, HttpRequestMessage request)
        {
            var urlHelper = new UrlHelper(request);
            return new RoleReturnModel
            {
                Url = urlHelper.Link("GetRoleById", new { id = role.Id }),
                Id = role.Id,
                Name = role.Name,
            };
        }

        public ClientReturnModel Create(Client client)
        {
            return new ClientReturnModel
            {
                Id = client.Id,
                Name = client.Name,
                Secret = client.Secret,
                Active = client.Active,
                AllowedOrigin = client.AllowedOrigin,
                ApplicationType = client.ApplicationType,
                RefreshTokenLifeTime = client.RefreshTokenLifeTime
            };
        }
    }
}