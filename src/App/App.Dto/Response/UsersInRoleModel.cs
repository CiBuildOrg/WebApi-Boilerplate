using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Dto.Response
{
    public class UsersInRoleModel
    {
        public Guid Id { get; set; }
        public List<Guid> EnrolledUsers { get; set; }
        public List<Guid> RemovedUsers { get; set; }
    }

    public class UserDto
    {
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }
    }
}