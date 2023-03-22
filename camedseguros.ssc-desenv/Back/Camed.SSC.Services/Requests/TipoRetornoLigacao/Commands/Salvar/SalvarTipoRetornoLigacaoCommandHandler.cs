using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoRetornoLigacaoCommandHandler : HandlerBase, IRequestHandler<SalvarTipoRetornoLigacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoRetornoLigacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoRetornoLigacaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoRetornoLigacao acao = new Domain.Entities.TipoRetornoLigacao();
                acao.Descricao = request.Descricao;
                await uow.GetRepository<Domain.Entities.TipoRetornoLigacao>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoRetornoLigacao>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                uow.GetRepository<Domain.Entities.TipoRetornoLigacao>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
