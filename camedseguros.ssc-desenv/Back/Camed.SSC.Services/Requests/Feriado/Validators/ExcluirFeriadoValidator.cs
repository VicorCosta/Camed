using FluentValidation;
using FluentValidation.Results;
using System;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirFeriadoValidator : AbstractValidator<ExcluirFeriadoCommand>
    {
        public ExcluirFeriadoValidator()
        {
            ValidatorId();

        }

        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }

        internal ValidationResult Validate(ExcluirFeriadoValidator validationRules)
        {
            throw new NotImplementedException();
        }
    }
}
