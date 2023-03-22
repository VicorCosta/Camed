using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarVinculoBNBValidator : AbstractValidator<SalvarVinculoBNBCommand>
    {
        public SalvarVinculoBNBValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Nome' é de 100 caracteres");

        }
    }
}
