using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using App.Api.Models;
using App.Dto.Request;
using App.Entities;
using App.Entities.Security;
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

        private readonly ModelFactory _factory;
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;
        private readonly RoleManager<ApplicationRole, Guid> _applicationRoleManager;

        public AccountsController(UserManager<ApplicationUser, Guid> applicationUserManager, RoleManager<ApplicationRole, Guid> applicationRoleManager)
        {
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
            _factory = new ModelFactory(_applicationUserManager);
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <returns>user list</returns>
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_applicationUserManager.Users.ToList().Select(u => _factory.Create(u, Request)));
        }

        /// <summary>
        /// Get user by guid
        /// </summary>
        /// <param name="id">user guid</param>
        /// <returns>user</returns>
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(Guid id)
        {
            var user = await _applicationUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(_factory.Create(user, Request));
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
            var user = await _applicationUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(_factory.Create(user, Request));
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

            var user = new ApplicationUser
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

            var result = await _applicationUserManager.CreateAsync(user, createUserModel.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var assignRole = _applicationRoleManager.FindByName("User");

            if (assignRole != null)
            {
                await _applicationUserManager.AddToRoleAsync(user.Id, assignRole.Name);
            }

            var locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, _factory.Create(user, Request));
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

            var result = await _applicationUserManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), 
                model.OldPassword, model.NewPassword);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("user/{id:guid}", Name = "DeleteUser")]
        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {
            // TODO: Only SuperAdmin or Admin can delete users.
            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var result = await _applicationUserManager.DeleteAsync(appUser);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
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
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] Guid id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel item in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == item.Type))
                {
                    await _applicationUserManager.RemoveClaimAsync(id, new Claim(item.Type, item.Value));
                }

                await _applicationUserManager.AddClaimAsync(id, new Claim(item.Type, item.Value, ClaimValueTypes.String));
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
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] Guid id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel item in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == item.Type))
                {
                    await _applicationUserManager.RemoveClaimAsync(id, new Claim(item.Type, item.Value));
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
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] Guid id, [FromBody] List<Guid> rolesToAssign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (var roleId in rolesToAssign)
            {
                var role = await _applicationRoleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    continue;
                }

                if (appUser.Roles.Any(r => r.RoleId == role.Id))
                {
                    await _applicationUserManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                }

                await _applicationUserManager.AddToRoleAsync(appUser.Id, role.Name);
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
        public async Task<IHttpActionResult> RemoveRolesFromUser([FromUri] Guid id, [FromBody] List<Guid> rolesToRemove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (var roleId in rolesToRemove)
            {
                var role = await _applicationRoleManager.FindByIdAsync(roleId);

                if (appUser.Roles.Any(r => r.RoleId == role.Id))
                {
                    await _applicationUserManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                }
            }

            return Ok();
        }
    }
}