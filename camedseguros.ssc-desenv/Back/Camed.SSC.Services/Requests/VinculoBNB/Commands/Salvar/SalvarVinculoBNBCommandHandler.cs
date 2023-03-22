using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarVinculoBNBCommandHandler : HandlerBase, IRequestHandler<SalvarVinculoBNBCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarVinculoBNBCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarVinculoBNBCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.VinculoBNB acao = new Domain.Entities.VinculoBNB();
                acao.Nome = request.Nome;
                await uow.GetRepository<Domain.Entities.VinculoBNB>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.VinculoBNB>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                uow.GetRepository<Domain.Entities.VinculoBNB>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
