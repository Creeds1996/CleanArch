using Application.Account.DTOs;
using Application.Core;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Account.Queries
{
    public class CurrentUser
    {
        public class Query : IRequest<Result<UserDto>>
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UserDto>>
        {
            private UserManager<AppUser> _userManager { get; }
            private readonly IMapper _mapper;
            private readonly ILogger<CurrentUser> _logger;
            private readonly TokenService _tokenService;

            public Handler(UserManager<AppUser> userManager, IMapper mapper, TokenService tokenService, ILogger<CurrentUser> logger)
            {
                _userManager = userManager;
                _mapper = mapper;
                _logger = logger;
                _tokenService = tokenService;
            }

            public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.Email == request.Email);

                if (user == null)
                {
                    _logger.LogInformation("Unable to find user with email: {email}", request.Email);
                    return Result<UserDto>.Failure("No user found with that provided email.");
                }

                var res = _mapper.Map<UserDto>(user);
                res.Token = await _tokenService.CreateToken(user);
                return Result<UserDto>.Success(res);
            }
        }
    }
}
