using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests
{
    public class SalvarInboxCommand : CommandBase, IRequest<IResult>
    {
        public int RemetenteId { get; set; }
        public string Assunto { get; set; }
        public string Texto { get; set; }
        public int[] Destinatarios { get; set; }
        public int IdMensagemOriginal { get; set; }
        public int? numeroSolicitacao { get; set; }
        public List<AnexoDeInboxViewModel> Anexos { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarInboxValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
