using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirFluxoDeOperacaoCommandHandler : HandlerBase, IRequestHandler<ExcluirFluxoDeOperacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService appIdentity;

        public ExcluirFluxoDeOperacaoCommandHandler(IUnitOfWork uow, IIdentityAppService appIdentity)
        {
            this.uow = uow;                           
            this.appIdentity = appIdentity;
        }

        public async Task<IResult> Handle(ExcluirFluxoDeOperacaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var mapeamento = await uow.GetRepository<MapeamentoAcaoSituacao>().GetByIdAsync(request.Id);

            if(mapeamento == null)
            {
                throw new ApplicationException("Fluxo não localizado na base dados.");
            }

            await uow.GetRepository<MapeamentoAcaoSituacao>().RemoveAndSaveAsync(mapeamento);


            return await Task.FromResult(result);
        }

    }
}
