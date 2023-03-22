using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Inbox.Commands.MarcarMensagemComoLida
{
    public class AtualizarLidaCommandHandler : HandlerBase, IRequestHandler<AtualizarLidaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public AtualizarLidaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IResult> Handle(AtualizarLidaCommand request, CancellationToken cancellationToken)
        {
            var mensagem = uow.GetRepository<Domain.Entities.Inbox>().GetByIdAsync(request.Id).Result;

            if (mensagem != null)
            {
                mensagem.Lida = true;
                uow.Commit();
            }
            return await Task.FromResult(result);
        }
    }
}
