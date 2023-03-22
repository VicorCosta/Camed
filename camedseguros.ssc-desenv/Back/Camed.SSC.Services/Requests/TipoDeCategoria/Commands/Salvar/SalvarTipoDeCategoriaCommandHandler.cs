using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeCategoriaCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeCategoriaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeCategoriaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeCategoriaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoDeCategoria acao = new Domain.Entities.TipoDeCategoria();
                acao.Descricao = request.Descricao;
                acao.TipoDeProduto = await uow.GetRepository<Domain.Entities.TipoDeProduto>().GetByIdAsync(request.TipoDeProduto) ?? throw new ApplicationException("Tipo de Produto nao foi encontrado.");
                await uow.GetRepository<Domain.Entities.TipoDeCategoria>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoDeCategoria>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                acao.TipoDeProduto = await uow.GetRepository<Domain.Entities.TipoDeProduto>().GetByIdAsync(request.TipoDeProduto) ?? throw new ApplicationException("Tipo de Produto nao foi encontrado.");
                uow.GetRepository<Domain.Entities.TipoDeCategoria>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
