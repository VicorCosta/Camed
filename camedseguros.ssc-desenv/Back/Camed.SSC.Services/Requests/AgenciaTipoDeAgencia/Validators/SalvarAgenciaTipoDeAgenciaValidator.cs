using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgenciaTipoDeAgenciaValidator : AbstractValidator<SalvarAgenciaTipoDeAgenciaCommand>
    {
        public SalvarAgenciaTipoDeAgenciaValidator()
        {
            ValidateAgencia();
            ValidateTipoDeAgencia();

        }

        private void ValidateAgencia()
        {
            RuleFor(r => r.AgenciaId)
                .NotNull().WithMessage("'Agência' é obrigatório");

        }
        private void ValidateTipoDeAgencia()
        {
            RuleFor(r => r.TipoDeAgenciaId)
                .NotNull().WithMessage("'Tipo de Agência' é obrigatório");

        }
    }
}
