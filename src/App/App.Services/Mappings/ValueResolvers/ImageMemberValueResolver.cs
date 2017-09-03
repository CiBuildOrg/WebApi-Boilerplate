using System.Collections.Generic;
using System.Linq;
using App.Core;
using App.Core.Contracts;
using App.Entities;
using AutoMapper;

namespace App.Services.Mappings.ValueResolvers
{
    public class ImageMemberValueResolver : IMemberValueResolver<object, object, ICollection<Image>, string>
    {
        private readonly IConfiguration _configuration;

        public ImageMemberValueResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string BaseUrl => _configuration.GetString(ConfigurationKeys.ApiPath);

        public string Resolve(object source, object destination, ICollection<Image> sourceMember, string destMember,
            ResolutionContext context)
        {
            var image = sourceMember.SingleOrDefault(x => x.ImageType == ImageType.Avatar && x.ImageSize == ImageSize.Small);
            if (image == null)
            {
                return $"{BaseUrl}/api/images/default";
            }

            return $"{BaseUrl}/api/images/{image.Id}";
        }
    }
}