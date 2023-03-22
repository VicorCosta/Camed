using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAreaDeNegocioCommandHandler : HandlerBase, IRequestHandler<SalvarAreaDeNegocioCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAreaDeNegocioCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAreaDeNegocioCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.AreaDeNegocio acao = new Domain.Entities.AreaDeNegocio();
                acao.Nome = request.Nome;
                await uow.GetRepository<Domain.Entities.AreaDeNegocio>().AddAsync(acao);
            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.AreaDeNegocio>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                uow.GetRepository<Domain.Entities.AreaDeNegocio>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
