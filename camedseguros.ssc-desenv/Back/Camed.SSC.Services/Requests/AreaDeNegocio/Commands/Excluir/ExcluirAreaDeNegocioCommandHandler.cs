using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirAreaDeNegocioCommandHandler : HandlerBase, IRequestHandler<ExcluirAreaDeNegocioCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirAreaDeNegocioCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirAreaDeNegocioCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.AreaDeNegocio>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.AreaDeNegocio>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Area de negocio não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }

    }
}
