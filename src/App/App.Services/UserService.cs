﻿using System;
using System.Collections.Generic;
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
        
        public NewUserResponse Register(NewUserDto request)
        {
            try
            {
                var userId = Guid.NewGuid();
                var imageId = Guid.NewGuid();

                var imageMemoryStream = _imageProcessorService.ProcessAvatar(request.Avatar.Buffer);
                var filename = imageId.ToString();

                _imageService.StoreImage(imageMemoryStream, imageId);

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
                    MimeType = ApplicationConstants.DefaultMimeType,
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
    }
}