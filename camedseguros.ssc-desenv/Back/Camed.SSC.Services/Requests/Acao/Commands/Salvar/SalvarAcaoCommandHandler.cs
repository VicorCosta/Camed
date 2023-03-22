using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAcaoCommandHandler : HandlerBase, IRequestHandler<SalvarAcaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAcaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAcaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Acao acao = new Domain.Entities.Acao();
                acao.Nome = request.Nome;
                acao.Descricao = request.Descricao;
                var papel = await uow.GetRepository<Papel>().GetByIdAsync(request.Papel) ?? throw new ApplicationException("Papel nao foi encontrado");
                acao.Papel = papel;
                
                await uow.GetRepository<Domain.Entities.Acao>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Acao>().GetByIdAsync(request.Id);
                acao.Nome = request.Nome;
                acao.Descricao = request.Descricao;
                var papel = await uow.GetRepository<Papel>().GetByIdAsync(request.Papel) ?? throw new ApplicationException("Papel nao foi encontrado");
                acao.Papel = papel;
                uow.GetRepository<Domain.Entities.Acao>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
