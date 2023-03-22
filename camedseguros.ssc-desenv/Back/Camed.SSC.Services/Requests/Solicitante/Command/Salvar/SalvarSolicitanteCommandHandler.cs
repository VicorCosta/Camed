using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitante.Command.Excluir
{
    public class SalvarSolicitanteCommandHandler : HandlerBase, IRequestHandler<SalvarSolicitanteCommand, IResult>
    {
        public Task<IResult> Handle(SalvarSolicitanteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
