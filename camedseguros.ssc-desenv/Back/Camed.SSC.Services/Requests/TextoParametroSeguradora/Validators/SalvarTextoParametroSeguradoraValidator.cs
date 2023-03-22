using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoParametroSeguradoraValidator : AbstractValidator<SalvarTextoParametroSeguradoraCommand>
    {
        public SalvarTextoParametroSeguradoraValidator()
        {
            ValidarSeguradora();
            ValidarTexto();
        }

        private void ValidarSeguradora()
        {
            RuleFor(r => r.Seguradora_Id)
                .NotEmpty().WithMessage("'Seguradora' é obrigatório")
                .NotNull().WithMessage("'Seguradora' é obrigatório");
        }
        private void ValidarTexto()
        {
            RuleFor(r => r.Texto)
                .NotEmpty().WithMessage("'Texto/HTML' é obrigatório");

        }
    }
}
