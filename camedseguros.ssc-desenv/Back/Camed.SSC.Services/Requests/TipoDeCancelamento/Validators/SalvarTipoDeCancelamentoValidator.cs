using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeCancelamentoValidator : AbstractValidator<SalvarTipoDeCancelamentoCommand>
    {
        public SalvarTipoDeCancelamentoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .MaximumLength(50).WithMessage("Tamanho máximo de 'Nome' é de 50 caracteres");
            RuleFor(r => r.GrupoAgencia)
                 .NotEmpty().WithMessage("'Grupo de Agencia' é obrigatório")
                 .NotNull().WithMessage("'Grupo de Agencia' é obrigatório");

        }
    }
}
