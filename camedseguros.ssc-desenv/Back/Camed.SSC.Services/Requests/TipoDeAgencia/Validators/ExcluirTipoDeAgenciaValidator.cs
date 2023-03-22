using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirTipoDeAgenciaValidator : AbstractValidator<ExcluirTipoDeAgenciaCommand>
    {
        public ExcluirTipoDeAgenciaValidator()
        {
            ValidatorId();

        }

        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }

    }
}
