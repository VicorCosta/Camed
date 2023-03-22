using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirTextoParametroSeguradoraCommandHandler : HandlerBase, IRequestHandler<ExcluirTextoParametroSeguradoraCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirTextoParametroSeguradoraCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirTextoParametroSeguradoraCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var seguradora = await uow.GetRepository<Domain.Entities.TextoParametroSeguradora>().GetByIdAsync(request.Id);

            if (seguradora != null)
            {
                await uow.GetRepository<Domain.Entities.TextoParametroSeguradora>().RemoveAndSaveAsync(seguradora);
            }
            else
            {
                result.Message = "Seguradora não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }

    }
}
