using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirCheckin
{
    public class AtribuirCheckinCommandHandler : HandlerBase, IRequestHandler<AtribuirCheckinCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public AtribuirCheckinCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(AtribuirCheckinCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().GetByIdAsync(request.Solicitacao).Result;

                if (solicitacao == null)
                    throw new ApplicationException("Solicitação informada não localizada na base de dados");

                foreach (var checkin in request.Checkins)
                {
                    var ck = uow.GetRepository<Checkin>().GetByIdAsync(checkin).Result;

                    if (ck == null)
                        throw new ApplicationException("Checkin selecionado não localizado na base de dados");

                    ck.Solicitacao = solicitacao;
                    await uow.CommitAsync();
                }

                solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == solicitacao.Id).Result;

                result.Payload = solicitacao;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
