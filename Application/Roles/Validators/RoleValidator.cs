using Application.Roles.Dto;
using FluentValidation;

namespace Application.Activities.Validators
{
    public class RoleValidator : AbstractValidator<UpdateRole>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Role).NotNull().NotEmpty();
        }
    }
}
