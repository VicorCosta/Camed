using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirMapeamentoAreaDeNegocioCommandHandler : HandlerBase, IRequestHandler<ExcluirMapeamentoAreaDeNegocioCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirMapeamentoAreaDeNegocioCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirMapeamentoAreaDeNegocioCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.MapeamentoAreaDeNegocio>()
                .QueryFirstOrDefaultAsync(w => w.Id == request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.MapeamentoAreaDeNegocio>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Mapeamento Área De Negócio não localizada na base de dados";
                return await Task.FromResult(result);
            }

            
            return await Task.FromResult(result);
        }

    }
}
