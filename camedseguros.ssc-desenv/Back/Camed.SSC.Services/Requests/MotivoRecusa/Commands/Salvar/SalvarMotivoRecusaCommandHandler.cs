using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMotivoRecusaCommandHandler : HandlerBase, IRequestHandler<SalvarMotivoRecusaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarMotivoRecusaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarMotivoRecusaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.MotivoRecusa acao = new Domain.Entities.MotivoRecusa();
                acao.Descricao = request.Descricao;
                acao.Ativo = request.Ativo;
                await uow.GetRepository<Domain.Entities.MotivoRecusa>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.MotivoRecusa>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                acao.Ativo = request.Ativo;
                uow.GetRepository<Domain.Entities.MotivoRecusa>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
