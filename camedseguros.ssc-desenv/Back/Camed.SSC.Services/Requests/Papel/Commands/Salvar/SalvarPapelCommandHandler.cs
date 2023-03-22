using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarPapelCommandHandler : HandlerBase, IRequestHandler<SalvarPapelCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarPapelCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarPapelCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Papel acao = new Domain.Entities.Papel();
                acao.Nome = request.Nome;
                acao.Descricao = request.Descricao;
                await uow.GetRepository<Domain.Entities.Papel>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Papel>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                acao.Descricao = request.Descricao;
                uow.GetRepository<Domain.Entities.Papel>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
