using Camed.SSC.Application.Requests.Solicitacao.Validators.InserirAcompanhamento;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.InserirAcompanhamento
{
    public class InserirAcompanhamentoCommand : CommandBase, IRequest<IResult>
    {
        public Situacao Situacao { get; set; }
        public Usuario Atendente { get; set; }
        public Grupo Grupo { get; set; }
        public int? TempoSLADef { get; set; }
        public string TempoSLAEfet { get; set; }
        public int Solicitacao_Id { get; set; }
        public int Acao_Id { get; set; }
        /*public Camed.SSC.Domain.Entities.Solicitacao Solicitacao { get; set; }*/

        public override bool IsValid()
        {
            ValidationResult = new InserirAcompanhamentoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
