using FluentValidation;

namespace AccountSeller.Application.UserManagement.Commands.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.MoveTime).NotNull();
            RuleFor(x => x.Name).MaximumLength(50);
            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x => x.Password).MaximumLength(14).When(x => !String.IsNullOrEmpty(x.Password));
            RuleFor(x => x.Password).MinimumLength(8).When(x => !String.IsNullOrEmpty(x.Password));
            RuleFor(x => x.Email).MaximumLength(60).When(x => x.Email != null);
        }
    }
}
