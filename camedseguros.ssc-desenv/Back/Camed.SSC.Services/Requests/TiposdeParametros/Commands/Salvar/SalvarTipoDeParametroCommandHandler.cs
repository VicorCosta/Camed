using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Application.Requests.TiposdeParametros.Commands.Salvar;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeParametroCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeParametroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeParametroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeParametroCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.TiposdeParametros acao = new Domain.Entities.TiposdeParametros();
                acao.Nome = request.Nome;

                await uow.GetRepository<Domain.Entities.TiposdeParametros>().AddAsync(acao);
            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.TiposdeParametros>().QueryFirstOrDefaultAsync(r => r.Id == request.Id);
                acao.Nome = request.Nome;


                uow.GetRepository<Domain.Entities.TiposdeParametros>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);


        }

    }
}
