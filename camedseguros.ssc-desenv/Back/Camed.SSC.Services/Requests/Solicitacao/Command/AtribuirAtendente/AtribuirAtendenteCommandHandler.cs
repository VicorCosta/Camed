using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirAtendente
{
    public class AtribuirAtendenteCommandHandler : HandlerBase, IRequestHandler<AtribuirAtendenteCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identity;

        public AtribuirAtendenteCommandHandler(IUnitOfWork uow, IIdentityAppService identity)
        {
            this.uow = uow;
            this.identity = identity;
        }

        public async Task<IResult> Handle(AtribuirAtendenteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var idAtendente = request.idAtendente ?? uow.GetRepository<Usuario>()
                    .GetByIdAsync(identity.Identity.Id).Result.Id;

                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>()
                    .QueryFirstOrDefaultAsync(x => x.Id == request.idSolicitacao).Result;

                if (solicitacao == null)
                    throw new ApplicationException("Solicitação não localizada na base de dados");

                solicitacao.Atendente = uow.GetRepository<Usuario>().GetByIdAsync(idAtendente).Result;

                if (request.idAreaDeNegocio.HasValue)
                {
                    solicitacao.AreaDeNegocio = uow.GetRepository<AreaDeNegocio>().GetByIdAsync(request.idAreaDeNegocio.Value).Result;
                }

                await uow.GetRepository<Domain.Entities.Solicitacao>().UpdateAndSaveAsync(solicitacao);
                await uow.CommitAsync();

                var sol = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == solicitacao.Id).Result;
                result.Payload = sol;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
