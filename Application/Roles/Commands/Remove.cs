using Application.Core;
using Application.Roles.Dto;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands
{
    public class Remove
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UpdateRole removeRole;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<AppUser> _userManager;

            public Handler(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
            {
                _roleManager = roleManager;
                _userManager = userManager;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.removeRole.Email);

                if (user == null) return Result<Unit>.Failure("Unable to find user.");

                var role = await _roleManager.RoleExistsAsync(request.removeRole.Role);

                if (!role) return Result<Unit>.Failure($"Unable to find role: {request.removeRole.Role}");

                var hasRole = await _userManager.IsInRoleAsync(user, request.removeRole.Role);

                if (!hasRole) return Result<Unit>.Failure($"User isn't in role: {request.removeRole.Role}");

                var result = await _userManager.RemoveFromRoleAsync(user, request.removeRole.Role);

                if (result.Succeeded) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure($"Problem removing user from role: {request.removeRole.Role}");
            }
        }
    }
}
