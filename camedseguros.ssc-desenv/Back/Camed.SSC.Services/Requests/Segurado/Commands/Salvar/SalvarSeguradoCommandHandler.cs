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
    public class SalvarSeguradoCommandHandler : HandlerBase, IRequestHandler<SalvarSeguradoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarSeguradoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarSeguradoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Segurado acao = new Domain.Entities.Segurado();
                //acao.Descricao = request.Descricao;
          
                await uow.GetRepository<Domain.Entities.Segurado>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Segurado>().GetByIdAsync(request.Id);
                //acao.Descricao = request.Se;
                uow.GetRepository<Domain.Entities.Segurado>().Update(acao);
            }

            await uow.CommitAsync();

            result.Successfully = true;
            return await Task.FromResult(result);
        }

    }
}
