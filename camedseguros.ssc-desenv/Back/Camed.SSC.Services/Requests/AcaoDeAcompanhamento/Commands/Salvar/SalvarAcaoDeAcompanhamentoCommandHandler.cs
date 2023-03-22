using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAcaoDeAcompanhamentoCommandHandler : HandlerBase, IRequestHandler<SalvarAcaoDeAcompanhamentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAcaoDeAcompanhamentoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAcaoDeAcompanhamentoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.AcaoDeAcompanhamento acao = new Domain.Entities.AcaoDeAcompanhamento();
                acao.Nome = request.Nome;
                
                await uow.GetRepository<Domain.Entities.AcaoDeAcompanhamento>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.AcaoDeAcompanhamento>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                uow.GetRepository<Domain.Entities.AcaoDeAcompanhamento>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
