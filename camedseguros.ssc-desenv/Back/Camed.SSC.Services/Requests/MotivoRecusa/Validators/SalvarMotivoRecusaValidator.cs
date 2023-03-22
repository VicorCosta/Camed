using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMotivoRecusaValidator : AbstractValidator<SalvarMotivoRecusaCommand>
    {
        public SalvarMotivoRecusaValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("'Descrição' é obrigatório")
                .NotNull().WithMessage("'Descrição' é obrigatório")
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Nome' é de 100 caracteres");

        }
    }
}
