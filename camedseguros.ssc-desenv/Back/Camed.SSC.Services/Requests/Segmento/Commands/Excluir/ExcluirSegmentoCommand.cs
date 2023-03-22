using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Segmento.Commands.Excluir
{
    public class ExcluirSegmentoCommand : CommandBase, IRequest<IResult>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
