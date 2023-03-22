using Camed.SSC.Application.Requests.Solicitacao.Validators.AtribuirCheckin;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirCheckin
{
    public class AtribuirCheckinCommand : CommandBase, IRequest<IResult>
    {
        public int[] Checkins { get; set; }
        public int Solicitacao { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AtribuirCheckinValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
