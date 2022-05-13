using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Roles.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<string>>>
        { }

        public class Handler : IRequestHandler<Query, Result<List<string>>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMapper _mapper;

            public Handler(RoleManager<IdentityRole> roleManager, IMapper mapper)
            {
                _roleManager = roleManager;
                _mapper = mapper;
            }

            public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var Roles = await _roleManager.Roles.ToListAsync();

                if (Roles == null) return Result<List<string>>.Failure("Currently no roles in DB.");

                var res = _mapper.Map<List<string>>(Roles);

                return Result<List<string>>.Success(res);
            }
        }
    }
}
