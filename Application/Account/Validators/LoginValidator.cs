using Application.Account.DTOs;
using FluentValidation;

namespace Application.Account
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}
