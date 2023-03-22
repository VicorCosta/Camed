using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoDeAtendenteCommandHandler : HandlerBase, IRequestHandler<SalvarMapeamentoDeAtendenteCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarMapeamentoDeAtendenteCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarMapeamentoDeAtendenteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.MapeamentoAtendente acao = new Domain.Entities.MapeamentoAtendente();
                acao.Atendente_Id = request.Atendente_Id.Value;
                acao.AreaDeNegocio_Id = request.AreaDeNegocio_Id.Value;
                acao.Agencia_Id = request.Agencia_Id.Value;
                acao.GrupoAgencia_Id = request.GrupoAgencia_Id.Value;
                acao.TipoDeSeguro_Id = request.TipoDeSeguro_Id.Value;

                await uow.GetRepository<Domain.Entities.MapeamentoAtendente>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.MapeamentoAtendente>().GetByIdAsync(request.Id);

                acao.Atendente_Id = request.Atendente_Id.Value;
                acao.AreaDeNegocio_Id = request.AreaDeNegocio_Id.Value;
                acao.Agencia_Id = request.Agencia_Id.Value;
                acao.GrupoAgencia_Id = request.GrupoAgencia_Id.Value;
                acao.TipoDeSeguro_Id = request.TipoDeSeguro_Id.Value;
                uow.GetRepository<Domain.Entities.MapeamentoAtendente>().Update(acao);
                }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
