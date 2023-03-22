using Camed.SSC.Application.Requests.Segmento.Commands.Salvar;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Segmento.Commands.Excluir
{
    public class ExcluirSegmentoCommandHandler : HandlerBase, IRequestHandler<ExcluirSegmentoCommand, IResult>
    {
        public Task<IResult> Handle(ExcluirSegmentoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
