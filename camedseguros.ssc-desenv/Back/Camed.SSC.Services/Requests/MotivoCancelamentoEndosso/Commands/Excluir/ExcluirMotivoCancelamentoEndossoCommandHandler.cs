using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirMotivoCancelamentoEndossoCommandHandler : HandlerBase, IRequestHandler<ExcluirMotivoCancelamentoEndossoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirMotivoCancelamentoEndossoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirMotivoCancelamentoEndossoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.MotivoEndossoCancelamento>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.MotivoEndossoCancelamento>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Acao de Acompanhamento não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
