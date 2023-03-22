using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeAgenciaValidator : AbstractValidator<SalvarTipoDeAgenciaCommand>
    {
        public SalvarTipoDeAgenciaValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(20).WithMessage("Tamanho máximo de 'Nome' é de 20 caracteres");
            RuleFor(r => r.GrupoAgencia).NotEmpty().NotNull();
        }
    }
}
