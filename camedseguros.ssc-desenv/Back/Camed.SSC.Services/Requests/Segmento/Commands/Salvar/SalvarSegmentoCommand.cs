using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Segmento.Commands.Salvar
{
    public class SalvarSegmentoCommand : CommandBase, IRequest<IResult>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
