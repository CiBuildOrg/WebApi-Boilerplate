using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Transactions;
using App.Core;
using App.Core.Contracts;
using App.Database;
using App.Dto.Request;
using App.Dto.Response;
using App.Entities;
using App.Entities.Security;
using App.Exceptions;
using App.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace App.Services
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly INow _now;
        private readonly DatabaseContext _context;
        private readonly IImageService _imageService;

        public UserService(UserManager<ApplicationUser, Guid> applicationUserManager, 
            IImageProcessorService imageProcessorService, INow now, DatabaseContext context, IImageService imageService)
        {
            _applicationUserManager = applicationUserManager;
            _imageProcessorService = imageProcessorService;
            _now = now;
            _context = context;
            _imageService = imageService;
        }

        public UserDto GetUser(Guid userId)
        {
            return null;
        }

        public RegistrationResult Register(NewUserDto request)
        {
            var userId = Guid.NewGuid();
            var imageId = Guid.NewGuid();

            var imageMemoryStream = _imageProcessorService.ProcessAvatar(request.Avatar.Buffer);
            var filename = imageId.ToString();
            _imageService.StoreImage(imageMemoryStream, imageId);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        Id = userId,
                        UserName = request.UserName,
                        Email = request.Email,
                        EmailConfirmed = true,

                        ProfileInfo = new UserProfile
                        {
                            FullName = request.FullName,
                            Description = request.Description,
                            JoinDate = DateTime.UtcNow,
                            ProfileImages = new List<Image>(),
                        }
                    };

                    var result = _applicationUserManager.Create(user, request.Password);
                    if (!result.Succeeded)
                    {
                        return new RegistrationResult { Success = false, Errors = result.Errors };
                    }

                    _applicationUserManager.SetLockoutEnabled(userId, false);
                    _applicationUserManager.AddToRoles(userId, Roles.User);

                    _context.SaveChanges();
                    // add the image to the database

                    var profile = _context.UserProfiles.SingleOrDefault(x => x.Id == userId);

                    if (profile == null)
                    {
                        throw new Exception();
                    }

                    var image = new Image
                    {
                        Id = imageId,
                        UserProfileId = profile.Id,
                        UserProfile = profile,
                        DateStoredUtc = _now.UtcNow,
                        FileName = filename,
                        ImageSize = ImageSize.Small,
                        MimeType = ApplicationConstants.DefaultMimeType,
                        ImageType = ImageType.Avatar
                    };

                    _context.Save(image);

                    scope.Complete();
                }
                catch
                {
                    scope.Dispose();

                    _imageService.TryDelete(imageId);

                    throw new RegistrationException(100, "User could not be registered");
                }
            }

            return RegistrationResult.Ok;
        }

        public bool UsernameAlreadyRegistered(string username)
        {
            return _context.Users.SingleOrDefault(x => x.UserName == username) != null;
        }

        public bool EmailAlreadyRegistered(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email) != null;
        }
    }
}