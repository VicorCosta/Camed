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
    public class MarcarMensagemComoLidaCommandHandler : HandlerBase, IRequestHandler<MarcarMensagemComoLidaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public MarcarMensagemComoLidaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IResult> Handle(MarcarMensagemComoLidaCommand request, CancellationToken cancellationToken)
        {
            var mensagem = uow.GetRepository<Domain.Entities.Inbox>().GetByIdAsync(request.Id).Result;


            if (mensagem != null )
            {
                mensagem.Lida = true;
                uow.Commit();
            }
            return await Task.FromResult(result);
        }
    }
}
