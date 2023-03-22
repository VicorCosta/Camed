using Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirAtendente;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.AtribuirAtendente
{
    public class AtribuirAtendenteValidator : AbstractValidator<AtribuirAtendenteCommand>
    {
        public AtribuirAtendenteValidator()
        {
            ValidateAtribuirAtendente();
        }

        private void ValidateAtribuirAtendente()
        {
            RuleFor(p => p.idSolicitacao)
                .NotNull().WithMessage("É necessário informar qual é a solicitação.")
                .NotEmpty().WithMessage("É necessário informar qual é a solicitação.");
        }
    }
}
