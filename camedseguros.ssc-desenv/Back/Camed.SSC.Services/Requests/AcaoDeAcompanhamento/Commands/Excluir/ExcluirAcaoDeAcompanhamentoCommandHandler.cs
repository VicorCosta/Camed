using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{

    public class ExcluirAcaoDeAcompanhamentoCommandHandler : HandlerBase, IRequestHandler<ExcluirAcaoDeAcompanhamentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirAcaoDeAcompanhamentoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirAcaoDeAcompanhamentoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.AcaoDeAcompanhamento>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "Frames","Frames.Frame" });

            if (acao != null)
            {
                if (acao.Frames.Any())
                {
                    var lista = acao.Frames.Select(s => s.Frame.Nome);
                    throw new ApplicationException($"Ação de acompanhamento contem relação com o(s) frame(s) {string.Join(", ", lista)}");
                }
                else
                {
                    await uow.GetRepository<Domain.Entities.AcaoDeAcompanhamento>().RemoveAndSaveAsync(acao);
                }
            }
            else
            {
                result.Message = "Acao de Acompanhamento não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
