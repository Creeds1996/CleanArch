using Application.Account.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Account.Commands
{
    public class Register
    {
        public class Command : IRequest<Result<UserDto>>
        {
            public RegisterDto registerDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.registerDto).SetValidator(new RegisterValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<UserDto>>
        {
            private ILogger<Login> _logger;
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper autoMapper, ILogger<Login> logger)
            {
                _logger = logger;
                _userManager = userManager;
                _signInManager = signInManager;
                _mapper = autoMapper;
            }

            public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _userManager.Users.AnyAsync(x => x.Email == request.registerDto.Email))
                {
                    _logger.LogInformation("Attempt to register with duplicate email: {Email}", request.registerDto.Email);
                    return Result<UserDto>.Failure("Email already in use.");
                }

                if (await _userManager.Users.AnyAsync(x => x.UserName == request.registerDto.Username))
                {
                    _logger.LogInformation("Attempt to register with taken username: {Username}", request.registerDto.Username);
                    return Result<UserDto>.Failure("Username already in use.");
                }

                var user = new AppUser
                {
                    DisplayName = request.registerDto.DisplayName,
                    Email = request.registerDto.Email,
                    UserName = request.registerDto.Username,
                };

                var result = await _userManager.CreateAsync(user, request.registerDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Successfully registered the user: {Username}", user.UserName);
                    return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
                }

                _logger.LogInformation("Error whilst registering user");
                return Result<UserDto>.Failure("Error whilst registering.");
            }
        }
    }
}
