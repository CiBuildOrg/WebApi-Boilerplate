using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using App.Entities;
using App.Entities.Security;
using App.Security.Infrastructure;
using Microsoft.AspNet.Identity;

namespace App.Database
{
    internal class DbSeed
    {
        private const string Secret = "asdf3235";
        private const string SuperAdminId = "01408ea9-766b-469d-9f09-f0acb07b3bf4";
        private const string AdminId = "141f10c1-1390-4ea0-85ba-b030fc0397b1";
        private const string UserId = "658f1036-bf08-45d4-a51b-9dbadb53498d";
        private const string DoruCUserId = "7F6C21AC-0201-44D1-8F9A-A92AF2B58AE8";

        private const string JavascriptClientApplicationId = "F1179B6B-15A8-4250-9ED9-4C2D5EE0376B";
        private const string MobileClientApplicationId = "3CFBC80C-9104-44E8-9E67-43663F25AC47";

        internal static void PopulateDatabase(DatabaseContext context)
        {
            var applicationUserManager = new UserManager<ApplicationUser, Guid>(new IdentityUserStore(context));
            var applicationRoleManager = new RoleManager<ApplicationRole, Guid>(new IdentityRoleStore(context));

            // seed the data

            if (!applicationRoleManager.Roles.Any(x => x.Name == "SuperAdmin"))
            {
                applicationRoleManager.Create(new ApplicationRole("SuperAdmin")
                {
                    Id = Guid.Parse(SuperAdminId),
                    RoleDescription = "super admin"
                });
            }

            if (!applicationRoleManager.Roles.Any(x => x.Name == "Admin"))
            {
                applicationRoleManager.Create(new ApplicationRole("Admin")
                {
                    Id = Guid.Parse(AdminId),
                    RoleDescription = "administrator"
                });
            }

            if (!applicationRoleManager.Roles.Any(x => x.Name == "User"))
            {
                applicationRoleManager.Create(new ApplicationRole("User")
                {
                    Id = Guid.Parse(UserId),
                    RoleDescription = "user"
                });
            }

            context.SaveChanges();

            var toAddUser = applicationUserManager.FindByName("doruc");
            if (toAddUser == null)
            {
                toAddUser = new ApplicationUser
                {
                    Id = Guid.Parse(DoruCUserId),
                    UserName = "doruc",
                    Email = "cioclea.doru@gmail.com",
                    EmailConfirmed = true,
                    ProfileInfo = new UserProfile
                    {
                        FullName = "Cioclea Doru",
                        Description = "Corebuild employee",
                        JoinDate = DateTime.UtcNow,
                    }
                };

                applicationUserManager.Create(toAddUser, Secret);
                applicationUserManager.SetLockoutEnabled(toAddUser.Id, false);
                applicationUserManager.AddToRoles(toAddUser.Id, "SuperAdmin", "Admin", "User");
            }

            // ReSharper disable once InvertIf
            if (!context.Clients.Any())
            {
                foreach (var item in BuildClientList())
                {
                    if (!context.Clients.Any(x => x.Id == item.Id))
                    {
                        context.Clients.Add(item);
                    }
                }
            }

            context.SaveChanges();
        }

        private static IEnumerable<Client> BuildClientList()
        {
            var clientList = new List<Client>
            {
                new Client
                {
                    Id = Guid.Parse(JavascriptClientApplicationId),
                    Secret = GetHash(Secret),
                    Name = "JavaScript Application",
                    ApplicationType = ApplicationType.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "*" // detail this more once you have a 
                },
                new Client
                {
                    Id = Guid.Parse(MobileClientApplicationId),
                    Secret = GetHash(Secret),
                    Name = "Mobile Application",
                    ApplicationType = ApplicationType.Mobile,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return clientList;
        }

        private static byte[] ToByteArray(string str)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            return strBytes;
        }

        private static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = ToByteArray(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}