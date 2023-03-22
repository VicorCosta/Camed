using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoPersonalizadoParametroCommandHandler : HandlerBase, IRequestHandler<SalvarTextoPersonalizadoParametroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTextoPersonalizadoParametroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTextoPersonalizadoParametroCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TextoParametrosSistema acao = new Domain.Entities.TextoParametrosSistema();
             
                acao.TipoDeSeguro_Id = request.TipoDeSeguro_Id.Value;
                acao.TipoDeProduto_Id = request.TipoDeProduto_Id.Value;
                acao.Texto = request.Texto;

                await uow.GetRepository<Domain.Entities.TextoParametrosSistema>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TextoParametrosSistema>().GetByIdAsync(request.Id);

                acao.TipoDeSeguro_Id = request.TipoDeSeguro_Id.Value;
                acao.TipoDeProduto_Id = request.TipoDeProduto_Id.Value;
                acao.Texto = request.Texto;

                uow.GetRepository<Domain.Entities.TextoParametrosSistema>().Update(acao);
                }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
