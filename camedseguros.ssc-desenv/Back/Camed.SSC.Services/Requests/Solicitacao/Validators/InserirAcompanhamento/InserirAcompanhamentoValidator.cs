using Camed.SSC.Application.Requests.Solicitacao.Command.InserirAcompanhamento;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.InserirAcompanhamento
{
    class InserirAcompanhamentoValidator : AbstractValidator<InserirAcompanhamentoCommand>
    {
        public InserirAcompanhamentoValidator()
        {
            ValidateInserirAcompanhamento();
        }

        private void ValidateInserirAcompanhamento()
        {
        }
    }
}
