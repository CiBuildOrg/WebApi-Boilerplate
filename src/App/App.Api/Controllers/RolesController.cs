using System;
using System.Threading.Tasks;
using System.Web.Http;
using App.Api.Models;
using App.Dto.Request;
using App.Dto.Response;
using App.Entities.Security;
using Microsoft.AspNet.Identity;

namespace App.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;
        private readonly RoleManager<ApplicationRole, Guid> _applicationRoleManager;
        private readonly ModelFactory _factory;

        public RolesController(UserManager<ApplicationUser, Guid> applicationUserManager, RoleManager<ApplicationRole, Guid> applicationRoleManager)
        {
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
            _factory = new ModelFactory(applicationUserManager);
        }
    
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(Guid id)
        {
            var role = await _applicationUserManager.FindByIdAsync(id);

            if (role != null)
            {
                return Ok(_factory.Create(role, Request));
            }

            return NotFound();

        }

        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _applicationRoleManager.Roles;

            return Ok(roles);
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new ApplicationRole { Name = model.Name };

            var result = await _applicationRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, _factory.Create(role, Request));

        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(Guid id)
        {

            var role = await _applicationRoleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _applicationRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await _applicationRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (var user in model.EnrolledUsers)
            {
                var appUser = await _applicationUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", $"User: {user} does not exists");
                    continue;
                }

                if (!_applicationUserManager.IsInRole(user, role.Name))
                {
                    var result = await _applicationUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", $"User: {user} could not be added to role");
                    }

                }
            }

            foreach (var user in model.RemovedUsers)
            {
                var appUser = await _applicationUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", $"User: {user} does not exists");
                    continue;
                }

                var result = await _applicationUserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", $"User: {user} could not be removed from role");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}