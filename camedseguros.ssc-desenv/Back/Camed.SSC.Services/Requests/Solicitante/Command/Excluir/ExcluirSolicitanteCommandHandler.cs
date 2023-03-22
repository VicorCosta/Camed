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
    public class ExcluirSolicitanteCommandHandler : HandlerBase, IRequestHandler<ExcluirSolicitanteCommand, IResult>
    {
        public Task<IResult> Handle(ExcluirSolicitanteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
