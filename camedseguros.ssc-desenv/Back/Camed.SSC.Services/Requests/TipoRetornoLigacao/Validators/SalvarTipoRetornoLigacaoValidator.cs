using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoRetornoLigacaoValidator : AbstractValidator<SalvarTipoRetornoLigacaoCommand>
    {
        public SalvarTipoRetornoLigacaoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Descricao)
                .NotNull().WithMessage("'Descricao' é obrigatório")
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Nome' é de 100 caracteres");

        }
    }
}
