using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCanalDeDistribuicaoCommandHandler : HandlerBase, IRequestHandler<SalvarCanalDeDistribuicaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarCanalDeDistribuicaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarCanalDeDistribuicaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.CanalDeDistribuicao acao = new Domain.Entities.CanalDeDistribuicao();
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                await uow.GetRepository<Domain.Entities.CanalDeDistribuicao>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.CanalDeDistribuicao>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                uow.GetRepository<Domain.Entities.CanalDeDistribuicao>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
