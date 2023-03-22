using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirAcaoCommandHandler : HandlerBase, IRequestHandler<ExcluirAcaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirAcaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirAcaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.Acao>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.Acao>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Ação não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }

    }
}
