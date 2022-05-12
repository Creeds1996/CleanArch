using Application.Account.DTOs;
using Application.Core;
using Application.Services;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistance;

namespace Application.Account.Commands
{
    public class Login
    {
        public class Command : IRequest<Result<UserDto>>
        {
            public LoginDto loginDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.loginDto).SetValidator(new LoginValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<UserDto>>
        {
            private ILogger<Login> _logger;
            private readonly DataContext _context;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IMapper _mapper;
            private readonly TokenService _tokenService;

            public Handler(DataContext context, SignInManager<AppUser> signInManager, IMapper autoMapper, ILogger<Login> logger, TokenService tokenService)
            {
                _logger = logger;
                _context = context;
                _signInManager = signInManager;
                _mapper = autoMapper;
                _tokenService = tokenService;
            }

            public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(a => a.Photos)
                    .FirstOrDefaultAsync(x => x.Email == request.loginDto.Email);

                if (user == null)
                {
                    _logger.LogInformation("Login attempt with non-existent user: {Email}", request.loginDto.Email);
                    return Result<UserDto>.Failure("Unable to find user with that username.");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.loginDto.Password, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("{User} has logged in.", user.DisplayName);
                    var res = _mapper.Map<UserDto>(user);
                    res.Token = _tokenService.CreateToken(user);
                    return Result<UserDto>.Success(res);
                }

                _logger.LogInformation("Failed login attempt for: {Email}", request.loginDto.Email);
                return Result<UserDto>.Failure("Invalid username or password.");
            }
        }
    }
}
