using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMotivoCancelamentoEndossoCommandHandler : HandlerBase, IRequestHandler<SalvarMotivoCancelamentoEndossoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarMotivoCancelamentoEndossoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarMotivoCancelamentoEndossoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }
            if (request.Id == 0)
            {
                Domain.Entities.MotivoEndossoCancelamento acao = new Domain.Entities.MotivoEndossoCancelamento();
                acao.Descricao = request.Descricao;
                await uow.GetRepository<Domain.Entities.MotivoEndossoCancelamento>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.MotivoEndossoCancelamento>().GetByIdAsync(request.Id);
                acao.Descricao = request.Descricao;
                uow.GetRepository<Domain.Entities.MotivoEndossoCancelamento>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
