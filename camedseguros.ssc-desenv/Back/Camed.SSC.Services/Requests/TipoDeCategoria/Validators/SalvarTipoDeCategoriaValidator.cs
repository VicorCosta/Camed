using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeCategoriaValidator : AbstractValidator<SalvarTipoDeCategoriaCommand>
    {
        public SalvarTipoDeCategoriaValidator()
        {
            ValidateNome();
            ValidateTipo();
        }

        private void ValidateNome()
        {
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("'Descricao' é obrigatório")
                .MaximumLength(30).WithMessage("Tamanho máximo de 'Nome' é de 30 caracteres");

        }

        private void ValidateTipo()
        {
            RuleFor(r => r.TipoDeProduto)
                .NotEmpty().WithMessage("'Tipo produto' é obrigatório")
                .NotNull().WithMessage("'Tipo produto' é obrigatório");

        }
    }
}
