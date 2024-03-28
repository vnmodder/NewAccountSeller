using FluentValidation;

namespace AccountSeller.Application.Authenticate.Signup
{
    public class SignupValidator : AbstractValidator<SignupRequest>
    {
        public SignupValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(14);
            RuleFor(x => x.Email).MaximumLength(60);
        }
    }
}
