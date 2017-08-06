using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using App.Dto.Request;
using App.Entities;
using App.Security;
using Microsoft.AspNet.Identity;

namespace App.Api.Controllers
{
    /// <summary>
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] 
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountsController : BaseApiController
    {
        /// <summary>
        /// Get users
        /// </summary>
        /// <returns>user list</returns>
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(AppUserManager.Users.ToList().Select(u => TheModelFactory.Create(u)));
        }

        /// <summary>
        /// Get user by guid
        /// </summary>
        /// <param name="id">user guid</param>
        /// <returns>user</returns>
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await AppUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();
        }

        /// <summary>
        /// Get roles
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public IHttpActionResult GetRoles()
        {
            return Ok(AppRoleManager.Roles.ToList().Select(r => TheModelFactory.Create(r)));
        }

        /// <summary>
        /// Get role by guid
        /// </summary>
        /// <param name="id">role guid</param>
        /// <returns>role</returns>
        [Authorize(Roles = "Admin")]
        [Route("role/{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await AppRoleManager.FindByIdAsync(id);

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        [Authorize(Roles = "Admin")]
        [Route("user/{username}", Name = "GetUserByName")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="createUserModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                EmailConfirmed = true,
                ProfileInfo = new UserProfile
                {
                    FullName = createUserModel.FullName,
                    JoinDate = DateTime.UtcNow,
                    Description = createUserModel.Description,
                }
            };

            var result = await AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var assignRole = AppRoleManager.FindByName("User");

            if (assignRole != null)
            {
                await AppUserManager.AddToRoleAsync(user.Id, assignRole.Name);
            }

            var locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [Route("changepassword", Name = "ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("user/{id:guid}", Name = "DeleteUser")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            // TODO: Only SuperAdmin or Admin can delete users.
            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                var result = await AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Assign claims to user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimsToAssign"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("user/{id:guid}/assignclaims")]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel item in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == item.Type))
                {
                    await AppUserManager.RemoveClaimAsync(id, new Claim(item.Type, item.Value));
                }

                await AppUserManager.AddClaimAsync(id, new Claim(item.Type, item.Value, ClaimValueTypes.String));
            }

            return Ok();
        }

        /// <summary>
        /// Remove claims from user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimsToRemove"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("user/{id:guid}/removeclaims")]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel item in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == item.Type))
                {
                    await AppUserManager.RemoveClaimAsync(id, new Claim(item.Type, item.Value));
                }
            }

            return Ok();
        }

        /// <summary>
        /// Assign roles to user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolesToAssign"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("user/{id:guid}/assignroles")]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] List<string> rolesToAssign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (var roleId in rolesToAssign)
            {
                var role = await AppRoleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    continue;
                }

                if (appUser.Roles.Any(r => r.RoleId == role.Id))
                {
                    await AppUserManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                }

                await AppUserManager.AddToRoleAsync(appUser.Id, role.Name);
            }

            return Ok();
        }

        /// <summary>
        /// Remove roles from user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolesToRemove"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("user/{id:guid}/removeroles")]
        public async Task<IHttpActionResult> RemoveRolesFromUser([FromUri] string id, [FromBody] List<string> rolesToRemove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (var roleId in rolesToRemove)
            {
                var role = await AppRoleManager.FindByIdAsync(roleId);

                if (appUser.Roles.Any(r => r.RoleId == role.Id))
                {
                    await AppUserManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                }
            }

            return Ok();
        }
    }
}