using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFrameCommandHandler : HandlerBase, IRequestHandler<SalvarFrameCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarFrameCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarFrameCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Frame acao = new Domain.Entities.Frame();
                acao.Nome = request.Nome;
                if (request.AcoesAcompanhamento != null)
                    acao.AcoesAcompanhamento.UpdateCollection(request.AcoesAcompanhamento.ToArray(), "AcaoDeAcompanhamento_Id");
                else
                    acao.AcoesAcompanhamento = null;
                await uow.GetRepository<Domain.Entities.Frame>().AddAsync(acao);
            }
            else
            {
                var ac = await uow.GetRepository<Domain.Entities.Frame>().QueryFirstOrDefaultAsync(r => r.Id == request.Id, includes: new[] { "AcoesAcompanhamento" });
                ac.Nome = request.Nome;
                ac.AcoesAcompanhamento.UpdateCollection(request.AcoesAcompanhamento.ToArray(), "AcaoDeAcompanhamento_Id");

                uow.GetRepository<Domain.Entities.Frame>().Update(ac);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
