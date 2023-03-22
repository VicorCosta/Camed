using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoMorteValidator : AbstractValidator<SalvarTipoMorteCommand>
    {
        public SalvarTipoMorteValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {      
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("Descricao é obrigatorio")
                .MaximumLength(100).WithMessage("Tamanho máximo de caractere da descrição é de 100");

        }
    }
}
