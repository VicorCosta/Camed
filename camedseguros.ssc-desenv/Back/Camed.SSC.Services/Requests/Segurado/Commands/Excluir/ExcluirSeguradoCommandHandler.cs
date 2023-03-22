using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirSeguradoCommandHandler : HandlerBase, IRequestHandler<ExcluirSeguradoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirSeguradoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirSeguradoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.Segurado>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.Segurado>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Segurado não localizada na base de dados";
                return await Task.FromResult(result);
            }

            result.Successfully = true;
            return await Task.FromResult(result);
        }

    }
}
