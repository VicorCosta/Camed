using System;
using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirAcaoDeAcompanhamentoCommandValidator : AbstractValidator<ExcluirAcaoDeAcompanhamentoCommand>
    {
        public ExcluirAcaoDeAcompanhamentoCommandValidator()
        {
            ValidatorId();
        }

        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }

    }
}
