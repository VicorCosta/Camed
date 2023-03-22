using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarGrupoAgenciaCommandHandler : HandlerBase, IRequestHandler<SalvarGrupoAgenciaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarGrupoAgenciaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarGrupoAgenciaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.GrupoAgencia acao = new Domain.Entities.GrupoAgencia();
                acao.Nome = request.Nome;
                await uow.GetRepository<Domain.Entities.GrupoAgencia>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                uow.GetRepository<Domain.Entities.GrupoAgencia>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
