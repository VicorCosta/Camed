using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeCancelamentoCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeCancelamentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeCancelamentoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeCancelamentoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoDeCancelamento acao = new Domain.Entities.TipoDeCancelamento();
                acao.Descricao = request.Descricao;
                acao.GrupoAgencia = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.GrupoAgencia) ?? throw new ApplicationException("Grupo de Agencia nao foi encontrado");
                await uow.GetRepository<Domain.Entities.TipoDeCancelamento>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoDeCancelamento>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                acao.GrupoAgencia = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.GrupoAgencia) ?? throw new ApplicationException("Grupo de Agencia nao foi encontrado");
                uow.GetRepository<Domain.Entities.TipoDeCancelamento>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
