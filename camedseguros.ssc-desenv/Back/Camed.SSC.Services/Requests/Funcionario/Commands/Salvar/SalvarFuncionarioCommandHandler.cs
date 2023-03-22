using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Funcionario.Commands.Salvar
{
    public class SalvarFuncionarioCommandHandler : HandlerBase, IRequestHandler<SalvarFuncionarioCommand, IResult>
    {
        public Task<IResult> Handle(SalvarFuncionarioCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
