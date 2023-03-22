using Camed.SSC.Application.Requests.Solicitacao.Command.AdicionarAcompanhamento;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.AdicionarAcompanhamento
{
    public class AdicionarAcompanhamentoValidator : AbstractValidator<AdicionarAcompanhamentoCommand>
    {
        public AdicionarAcompanhamentoValidator()
        {
            ValidateAnexoDeSolicitacao();
        }
        private void ValidateAnexoDeSolicitacao()
        {
            RuleFor(r => r.acompanhamento.Observacao)
           .NotNull().WithMessage("'Nome' é obrigatório");
        }
    }
}
