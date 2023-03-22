using Camed.SSC.Application.Requests.TiposdeParametros.Commands.Salvar;
using FluentValidation;



namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeParametrosValidator : AbstractValidator<SalvarTipoDeParametroCommand>
    {
        public SalvarTipoDeParametrosValidator()
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