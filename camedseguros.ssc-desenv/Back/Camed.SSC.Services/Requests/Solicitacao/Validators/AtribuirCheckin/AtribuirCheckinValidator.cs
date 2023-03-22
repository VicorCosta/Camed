using Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirCheckin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.AtribuirCheckin
{
    public class AtribuirCheckinValidator : AbstractValidator<AtribuirCheckinCommand>
    {
        public AtribuirCheckinValidator()
        {
            ValidateAtribuirChekin();
        }

        private void ValidateAtribuirChekin()
        {
            RuleFor(p => p.Solicitacao)
                .NotNull().WithMessage("É necessário informar qual é a solicitação.")
                .NotEmpty().WithMessage("É necessário informar qual é a solicitação.");
        }
    }
}
