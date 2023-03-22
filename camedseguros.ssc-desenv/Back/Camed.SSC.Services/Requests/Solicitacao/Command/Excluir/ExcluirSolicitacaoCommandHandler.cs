using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.Excluir
{
    public class ExcluirSolicitacaoCommandHandler : HandlerBase, IRequestHandler<ExcluirSolicitacaoCommand, IResult>
    {
        public Task<IResult> Handle(ExcluirSolicitacaoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
