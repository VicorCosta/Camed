using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoMorteCommandHandler : HandlerBase, IRequestHandler<SalvarTipoMorteCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoMorteCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoMorteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoMorte acao = new Domain.Entities.TipoMorte();
                acao.Descricao = request.Descricao;
          
                await uow.GetRepository<Domain.Entities.TipoMorte>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoMorte>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                uow.GetRepository<Domain.Entities.TipoMorte>().Update(acao);
            }

            await uow.CommitAsync();

            result.Successfully = true;
            return await Task.FromResult(result);
        }

    }
}
