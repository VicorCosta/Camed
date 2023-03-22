using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Salvar
{
    public class SalvarAvAtendimentoCommandHandler : HandlerBase, IRequestHandler<SalvarAvAtendimentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAvAtendimentoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAvAtendimentoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            Domain.Entities.AvAtendimento acao = new Domain.Entities.AvAtendimento();
            acao.Nota = request.Nota;
            acao.DataAvaliacao = request.DataAvaliacao;
            acao.Observacao = request.Observacao;
            acao.Solicitacao_Id = request.Solicitacao_Id;
            acao.Usuario_Id = request.Usuario_Id;

            await uow.GetRepository<Domain.Entities.AvAtendimento>().AddAsync(acao);

            await uow.CommitAsync();

            return await Task.FromResult(result);

        }
    }
}
