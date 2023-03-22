using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Excluir
{
    public class ExcluirAvAtendimentoCommandHandler : HandlerBase, IRequestHandler<ExcluirAvAtendimentoCommand, IResult>
    {
        public Task<IResult> Handle(ExcluirAvAtendimentoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
