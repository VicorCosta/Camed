using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCampanhaCommandHandler : HandlerBase, IRequestHandler<SalvarCampanhaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarCampanhaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarCampanhaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Campanha acao = new Domain.Entities.Campanha();
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                await uow.GetRepository<Domain.Entities.Campanha>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Campanha>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                uow.GetRepository<Domain.Entities.Campanha>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
