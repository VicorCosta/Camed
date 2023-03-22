using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirFrameCommandHandler : HandlerBase, IRequestHandler<ExcluirFrameCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirFrameCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirFrameCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.Frame>().QueryFirstOrDefaultAsync(r=>r.Id== request.Id, includes:new[] { "AcoesAcompanhamento", "AcoesAcompanhamento.AcaoDeAcompanhamento" }) ?? throw new ApplicationException($"Frame não foi encontrado com Id: {request.Id}");

            if (acao != null)
            {

                if (acao.AcoesAcompanhamento.Any())
                {
                    var lista = acao.AcoesAcompanhamento.Select(s=>s.AcaoDeAcompanhamento.Nome);
                    throw new ApplicationException($"Frame contém relação com a(s) ação/açôes de acompanhamento: '{string.Join(", ", lista)}'");
                }
                await uow.GetRepository<Domain.Entities.Frame>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Frame não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
