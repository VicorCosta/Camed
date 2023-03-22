using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCanalDeDistribuicaoValidator : AbstractValidator<SalvarCanalDeDistribuicaoCommand>
    {
        public SalvarCanalDeDistribuicaoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(30).WithMessage("Tamanho máximo do 'Nome' é de 30 caracteres");

        }
    }
}
