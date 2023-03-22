using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFrameValidator : AbstractValidator<SalvarFrameCommand>
    {
        public SalvarFrameValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Nome' é de 100 caracteres");

        }
    }
}
