using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirTipoDeProdutoCommandHandler : HandlerBase, IRequestHandler<ExcluirTipoDeProdutoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirTipoDeProdutoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirTipoDeProdutoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.TipoDeProduto>()
                .QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "Situacao", "SituacaoRenovacao"});

            if (acao != null)
            {
                acao.Situacao = null;
                acao.SituacaoRenovacao = null;

                await uow.GetRepository<Domain.Entities.TipoDeProduto>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Tipo de produto não localizada na base de dados";
                return await Task.FromResult(result);
            }

            
            return await Task.FromResult(result);
        }

    }
}
