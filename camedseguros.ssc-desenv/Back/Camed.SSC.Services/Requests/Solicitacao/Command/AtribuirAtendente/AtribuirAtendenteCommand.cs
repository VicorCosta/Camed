using Camed.SSC.Application.Requests.Solicitacao.Validators.AtribuirAtendente;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirAtendente
{
    public class AtribuirAtendenteCommand : CommandBase, IRequest<IResult>
    {
        public int idSolicitacao { get; set; }
        public int? idAtendente { get; set; }
        public int? idAreaDeNegocio { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AtribuirAtendenteValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
