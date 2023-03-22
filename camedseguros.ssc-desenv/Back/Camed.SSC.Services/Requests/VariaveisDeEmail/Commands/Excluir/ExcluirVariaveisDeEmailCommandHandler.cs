using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Camed.SSC.Application.Requests
{
   public class ExcluirVariaveisDeEmailCommandHandler : HandlerBase , IRequestHandler<ExcluirVariaveisDeEmailCommand , IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirVariaveisDeEmailCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirVariaveisDeEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }
            var acao = await uow.GetRepository<Domain.Entities.VariaveisDeEmail>().GetByIdAsync(request.Id);

            if(acao != null)
            {
                await uow.GetRepository<Domain.Entities.VariaveisDeEmail>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Variáveis de email não localizada na base de dados";
                return await Task.FromResult(result);
            }
            return await Task.FromResult(result);
        }
    }
}
