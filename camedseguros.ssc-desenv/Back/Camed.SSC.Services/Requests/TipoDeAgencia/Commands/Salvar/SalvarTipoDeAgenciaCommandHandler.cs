using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeAgenciaCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeAgenciaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeAgenciaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeAgenciaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoDeAgencia acao = new Domain.Entities.TipoDeAgencia();
                acao.Nome = request.Nome;
                acao.GrupoAgencia = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.GrupoAgencia) ?? 
                                            throw new ApplicationException("Grupo de Agencia não foi encontrado");
                await uow.GetRepository<Domain.Entities.TipoDeAgencia>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoDeAgencia>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                acao.GrupoAgencia = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.GrupoAgencia) ??
                                           throw new ApplicationException("Grupo de Agencia não foi encontrado");
                uow.GetRepository<Domain.Entities.TipoDeAgencia>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
