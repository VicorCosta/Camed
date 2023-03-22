using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarGrupoValidator : AbstractValidator<SalvarGrupoCommand>
    {
        public SalvarGrupoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .MaximumLength(20).WithMessage("Tamanho máximo de 'Nome' é de 20 caracteres");

        }
    }
}
