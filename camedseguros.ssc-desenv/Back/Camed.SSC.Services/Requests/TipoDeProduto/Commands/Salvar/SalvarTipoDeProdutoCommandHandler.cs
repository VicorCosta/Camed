using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeProdutoCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeProdutoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeProdutoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeProdutoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TipoDeProduto acao = new Domain.Entities.TipoDeProduto();

                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                acao.SlaMaximo = request.SlaMaximo;
                acao.UsoInterno = request.UsoInterno;
                acao.DescricaoSasParaTipoDeProduto = request.DescricaoSasParaTipoDeProduto;

                var situacao_bd = await uow.GetRepository<Situacao>().GetByIdAsync(request.Situacao);
                var situacaoRenovacao_bd = await uow.GetRepository<Situacao>().GetByIdAsync(request.SituacaoRenovacao);

                acao.Situacao = situacao_bd ?? throw new ApplicationException("Situação selecionada não existe.");
                acao.SituacaoRenovacao = situacaoRenovacao_bd;
                //acao.SituacaoRenovacao = situacaoRenovacao_bd ?? throw new ApplicationException("Situação selecionada não existe.");

                await uow.GetRepository<Domain.Entities.TipoDeProduto>().AddAsync(acao);

            }

            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TipoDeProduto>().GetByIdAsync(request.Id);

                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                acao.SlaMaximo = request.SlaMaximo;
                acao.UsoInterno = request.UsoInterno;
                acao.DescricaoSasParaTipoDeProduto = request.DescricaoSasParaTipoDeProduto;

                var situacao_bd = await uow.GetRepository<Situacao>().GetByIdAsync(request.Situacao);
                var situacaoRenovacao_bd = await uow.GetRepository<Situacao>().GetByIdAsync(request.SituacaoRenovacao);

                acao.Situacao = situacao_bd;
                acao.SituacaoRenovacao = situacaoRenovacao_bd;

                uow.GetRepository<Domain.Entities.TipoDeProduto>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
