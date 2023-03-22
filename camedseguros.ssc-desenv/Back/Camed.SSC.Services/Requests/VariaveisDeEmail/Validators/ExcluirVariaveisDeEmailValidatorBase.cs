using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirVariaveisDeEmail : AbstractValidator<ExcluirVariaveisDeEmailCommand>
    {
        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }
    }
}