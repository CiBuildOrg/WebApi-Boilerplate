﻿using App.Security.Validation;
using Autofac;
using Microsoft.AspNet.Identity;

namespace App.Security
{
    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register<PasswordValidator>(x => new CustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            });
        }
    }
}
