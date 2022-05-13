using Application.Roles.Commands;
using Application.Roles.Dto;
using Application.Roles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class RolesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole(UpdateRole addRole)
        {
            return HandleResult(await Mediator.Send(new Add.Command { addRole = addRole }));
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole(UpdateRole removeRole)
        {
            return HandleResult(await Mediator.Send(new Remove.Command { removeRole = removeRole }));
        }
    }
}
