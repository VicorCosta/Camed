using Camed.SSC.Application.Requests.AvaliacaoAtendimento.Validators;
using Camed.SSC.Application.Requests.Solicitacao.Validators.Salvar;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Salvar
{
    public class SalvarAvAtendimentoCommand : CommandBase, IRequest<IResult>
    {
        public string Observacao { get; set; }
        public string Nota { get; set; }
        public int Solicitacao_Id { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int Usuario_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarAvAtendimentoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
