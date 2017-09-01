using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Core;
using App.Core.Contracts;
using App.Database;
using App.Dto.Request;
using App.Dto.Response;
using App.Entities;
using App.Entities.Security;
using App.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly IStorageProvider _storageProvider;
        private readonly INow _now;
        private readonly DatabaseContext _context;

        public UserService(UserManager<ApplicationUser, Guid> applicationUserManager, 
            IImageProcessorService imageProcessorService, IStorageProvider storageProvider, INow now, DatabaseContext context)
        {
            _applicationUserManager = applicationUserManager;
            _imageProcessorService = imageProcessorService;
            _storageProvider = storageProvider;
            _now = now;
            _context = context;
        }
        
        public NewUserResponse Register(NewUserDto request)
        {
            try
            {
                var userId = Guid.NewGuid();
                var imageId = Guid.NewGuid();

                var imageMemoryStream = _imageProcessorService.ProcessAvatar(request.Avatar.Buffer);
                var extension = MimeTypeMap.List.MimeTypeMap.GetExtension(ApplicationConstants.DefaultMimeType).First();
                var filename = imageId.ToString();

                var imagePath =
                    HttpContext.Current.Server.MapPath(string.Format(ApplicationConstants.ImagePathTemplate,
                        ApplicationConstants.DefaultUserSubDirectory,
                        ReturnSizeSubdirectory(ImageSize.Small),
                        filename, extension));

                _storageProvider.StoreFile(imagePath, imageMemoryStream);

                var user = new ApplicationUser
                {
                    Id = userId,
                    UserName = request.UserName,
                    Email = request.Email,
                    EmailConfirmed = true,
                    ProfileInfo = new UserProfile
                    {
                        Id = userId,
                        FullName = request.FullName,
                        Description = request.Description,
                        JoinDate = DateTime.UtcNow,
                        ProfileImages = new List<Image>()
                    }
                };

                var image = new Image
                {
                    Id = imageId,
                    UserProfile = user.ProfileInfo,
                    UserProfileId = user.ProfileInfo.Id,
                    DateStoredUtc = _now.UtcNow,
                    FileName = filename,
                    ImageSize = ImageSize.Small,
                    MimeType = extension,
                    ImageType = ImageType.Avatar
                };

                _context.Save(image);

                _applicationUserManager.Create(user, request.Password);
                _applicationUserManager.SetLockoutEnabled(userId, false);
                _applicationUserManager.AddToRoles(userId, Roles.User);

                return new NewUserResponse
                {
                    Success = true,
                    Error = null
                };
            }

            catch (Exception ex)
            {
                return new NewUserResponse
                {
                    Error = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Returns the size directory ( small/medium/large )
        /// </summary>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        private static string ReturnSizeSubdirectory(ImageSize imageSize)
        {
            switch (imageSize)
            {
                case ImageSize.Small:
                    return "small";
                case ImageSize.Medium:
                    return "medium";
                case ImageSize.Large:
                    return "large";
                default: throw new InvalidOperationException($"Image size {imageSize} does not exist");
            }
        }
    }
}