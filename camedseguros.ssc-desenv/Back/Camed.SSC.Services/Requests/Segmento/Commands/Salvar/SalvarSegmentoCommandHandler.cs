using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Segmento.Commands.Salvar
{
    public class SalvarSegmentoCommandHandler : HandlerBase, IRequestHandler<SalvarSegmentoCommand, IResult>
    {
        public Task<IResult> Handle(SalvarSegmentoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
