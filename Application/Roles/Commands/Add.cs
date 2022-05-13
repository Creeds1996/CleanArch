using Application.Core;
using Application.Roles.Dto;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands
{
    public class Add
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UpdateRole addRole { get; set; }
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
                var user = await _userManager.FindByEmailAsync(request.addRole.Email);

                if (user == null) return Result<Unit>.Failure("Unable to find user.");

                var role = await _roleManager.RoleExistsAsync(request.addRole.Role);

                if (!role) return Result<Unit>.Failure($"Unable to find that role: {request.addRole.Role}");

                var hasRole = await _userManager.IsInRoleAsync(user, request.addRole.Role);

                if (hasRole) return Result<Unit>.Failure($"User is already in role: {request.addRole.Role}");

                var result = await _userManager.AddToRoleAsync(user, request.addRole.Role);

                if (result.Succeeded) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure($"Problem adding user to role: {request.addRole.Role}");
            }
        }
    }
}
