using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Queries
{
    public class Details
    {
        public class Query : IRequest<Result<List<string>>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<string>>>
        {
            private readonly UserManager<AppUser> _userManager;

            public Handler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null) return Result<List<string>>.Failure($"Unable to find user: {request.Username}");

                var roles = await _userManager.GetRolesAsync(user);

                if (roles == null || !roles.Any())
                {
                    return Result<List<string>>.Failure($"{request.Username} has no roles.");
                }

                return Result<List<string>>.Success(roles.ToList());
            }
        }
    }
}
