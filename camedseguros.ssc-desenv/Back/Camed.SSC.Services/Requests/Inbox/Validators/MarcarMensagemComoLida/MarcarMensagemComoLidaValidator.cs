using Camed.SSC.Application.Requests.Inbox.Commands.MarcarMensagemComoLida;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Validators.MarcarMensagemComoLida
{
    public class MarcarMensagemComoLidaValidator : AbstractValidator<MarcarMensagemComoLidaCommand>
    {
        public MarcarMensagemComoLidaValidator()
        {
            ValidateMensagem();
        }

        private void ValidateMensagem()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("É necessário informar o ID")
                .NotNull().WithMessage("É necessário informar o ID");
        }
    }
}
