using Application.Account.DTOs;
using FluentValidation;

namespace Application.Account
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must contain atleast 6 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain atleast one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain atleast one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain atleast one number.");
        }
    }
}
