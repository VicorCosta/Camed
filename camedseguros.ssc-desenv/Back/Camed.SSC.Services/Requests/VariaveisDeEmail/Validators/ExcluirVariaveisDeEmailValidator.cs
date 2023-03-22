using FluentValidation;
using FluentValidation.Results;
using System;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirVariaveisDeEmailValidator : AbstractValidator<ExcluirVariaveisDeEmailCommand>
    {
        public ExcluirVariaveisDeEmailValidator()
        {
            ValidatorId();
        }
        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }
    }
}
