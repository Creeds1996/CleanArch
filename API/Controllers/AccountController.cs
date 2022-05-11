using Application.Account.Commands;
using Application.Account.DTOs;
using Application.Account.Queries;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        public AccountController()
        {
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            return HandleResult(await Mediator.Send(new Login.Command { loginDto = loginDto}));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            return HandleResult(await Mediator.Send(new Register.Command { registerDto = registerDto }));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            return HandleResult(await Mediator.Send(new CurrentUser.Query { Email = User.FindFirstValue(ClaimTypes.Email) }));
        }
    }
}
