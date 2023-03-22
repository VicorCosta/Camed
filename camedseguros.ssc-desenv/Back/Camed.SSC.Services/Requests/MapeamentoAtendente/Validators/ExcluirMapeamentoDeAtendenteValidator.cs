using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirMapeamentoDeAtendenteValidator : AbstractValidator<ExcluirMapeamentoDeAtendenteCommand>
    {
        public ExcluirMapeamentoDeAtendenteValidator()
        {
            ValidatorId();

        }

        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }

    }
}
