using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            ValidateLoginWhenNull();
            ValidateLoginWhenEmpty();

            ValidatePasswordWhenNull();
            ValidatePasswordWhenEmpty();
        }

        void ValidateLoginWhenNull()
        {
            RuleFor(r => r.UserName)
                .NotNull().WithMessage("Campo obrigatório");
        }

        void ValidateLoginWhenEmpty()
        {
            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("Campo obrigatório").When(w => w.UserName != null);
        }

        void ValidatePasswordWhenNull()
        {
            RuleFor(r => r.Password)
                .NotNull().WithMessage("Campo obrigatório");
        }

        void ValidatePasswordWhenEmpty()
        {
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Campo obrigatório").When(w => w.Password != null);
        }
    }
}
