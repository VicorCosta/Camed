using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarParametroSistemaValidator : AbstractValidator<SalvarParametroSistemaCommand>
    {
        public SalvarParametroSistemaValidator()
        {
            ValidateParametro();
         }

        private void ValidateParametro()
        {
            RuleFor(r => r.Parametro)
                .NotNull().WithMessage("'Parâmetro' é obrigatório.");
        }
    }
}
