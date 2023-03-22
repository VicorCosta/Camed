using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeSeguroCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeSeguroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeSeguroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeSeguroCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoDeSeguro acao = new Domain.Entities.TipoDeSeguro();
                acao.Nome = request.Nome;
                var grupo = await uow.GetRepository<GrupoAgencia>().GetByIdAsync(request.GrupoAgenciaID);
                acao.GrupoAgencia = grupo ?? throw new System.ApplicationException("O grupo não foi localizado");

                if (request.RamosDeSeguro != null)
                {
                    acao.TiposDeProduto.UpdateCollection(request.RamosDeSeguro, keyProperty: "TipoDeProduto_Id");
                }

                await uow.GetRepository<Domain.Entities.TipoDeSeguro>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoDeSeguro>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "TiposDeProduto", "TiposDeProduto.TipoDeProduto" });
                acao.Nome = request.Nome;
                var grupo = await uow.GetRepository<GrupoAgencia>().GetByIdAsync(request.GrupoAgenciaID);
                acao.GrupoAgencia = grupo ?? throw new System.ApplicationException("O grupo não foi localizado");
                acao.TiposDeProduto.UpdateCollection(request.RamosDeSeguro, keyProperty: "TipoDeProduto_Id");

                uow.GetRepository<Domain.Entities.TipoDeSeguro>().Update(acao);
            }

            await uow.CommitAsync();


            return await Task.FromResult(result);
        }

    }
}
