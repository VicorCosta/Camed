using Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Salvar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.AvaliacaoAtendimento.Validators
{
    public class SalvarAvAtendimentoValidator : AbstractValidator<SalvarAvAtendimentoCommand>
    {
        public SalvarAvAtendimentoValidator()
        {
            ValidateSalvarAvAtendimento();
        }

        private void ValidateSalvarAvAtendimento()
        {
            RuleFor(p => p.Nota)
                .NotEmpty().WithMessage("É preciso informar a nota do atendimento")
                .NotNull().WithMessage("É preciso informar a nota do atendimento");
        }
    }
}
