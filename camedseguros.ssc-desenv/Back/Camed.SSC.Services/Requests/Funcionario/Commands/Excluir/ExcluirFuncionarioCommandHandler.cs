using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Funcionario.Commands.Excluir
{
    public class ExcluirFuncionarioCommandHandler : HandlerBase, IRequestHandler<ExcluirFuncionarioCommand, IResult>
    {
        public Task<IResult> Handle(ExcluirFuncionarioCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
