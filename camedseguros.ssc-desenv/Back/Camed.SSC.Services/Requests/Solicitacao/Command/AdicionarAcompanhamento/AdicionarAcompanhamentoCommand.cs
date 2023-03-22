using Camed.SSC.Application.Requests.Solicitacao.Validators.AdicionarAcompanhamento;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AdicionarAcompanhamento
{
    public class AdicionarAcompanhamentoCommand : CommandBase, IRequest<IResult>
    {
        public AcompanhamentoViewModel acompanhamento { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AdicionarAcompanhamentoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
