using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using App.Database.Security;
using App.Entities;
using App.Entities.Security;
using Microsoft.AspNet.Identity;

namespace App.Database
{
    public class ContextConfiguration : DbMigrationsConfiguration<DatabaseContext>
    {
        /// <summary>
        /// this basically tells no to entity framework running migrations automatically by itself
        /// </summary>
        public ContextConfiguration()
        {
            
            // disable automatic migrations
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            var applicationUserManager = new UserManager<ApplicationUser, Guid>(new IdentityUserStore(context));
            var applicationRoleManager = new RoleManager<CustomRole, Guid>(new IdentityRoleStore(context));

            // seed the data

            if (!applicationRoleManager.Roles.Any(x => x.Name == "SuperAdmin"))
            {
                applicationRoleManager.Create(new CustomRole("SuperAdmin") { Name = "super power admin" });
            }

            if (!applicationRoleManager.Roles.Any(x => x.Name == "Admin"))
            {
                applicationRoleManager.Create(new CustomRole("Admin") { Name = "administrator" });
            }

            if (!applicationRoleManager.Roles.Any(x => x.Name == "User"))
            {
                applicationRoleManager.Create(new CustomRole("User") { Name = "user" });
            }

            var toAddUser = applicationUserManager.FindByName("doruc");
            if (toAddUser == null)
            {
                toAddUser = new ApplicationUser
                {
                    Id = Guid.Parse("7F6C21AC-0201-44D1-8F9A-A92AF2B58AE8"),
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
                    context.Clients.Add(item);
                }

                context.SaveChanges();
            }
        }

        private const string Secret = "asdf3235";

        private IEnumerable<Client> BuildClientList()
        {
            var clientList = new List<Client>
            {
                new Client
                {
                    Id = Guid.Parse("F1179B6B-15A8-4250-9ED9-4C2D5EE0376B"),
                    Secret = GetHash(Secret),
                    Name = "JavaScript Application",
                    ApplicationType = ApplicationType.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "*" // detail this more once you have a 
                },
                new Client
                {
                    Id = Guid.Parse("3CFBC80C-9104-44E8-9E67-43663F25AC47"),
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

        private string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = ToByteArray(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
    