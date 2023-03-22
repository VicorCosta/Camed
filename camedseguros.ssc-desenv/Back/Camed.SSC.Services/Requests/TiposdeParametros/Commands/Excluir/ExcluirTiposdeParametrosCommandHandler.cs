using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.TiposdeParametros.Commands.Excluir
{
    class ExcluirTiposdeParametrosCommandHandler : HandlerBase, IRequestHandler<ExcluirTiposdeParametrosCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirTiposdeParametrosCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IResult> Handle(ExcluirTiposdeParametrosCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.TiposdeParametros>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.TiposdeParametros>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Parâmetro não localizado na base de dados";
                return await Task.FromResult(result);
            }


            return await Task.FromResult(result);
                    }
    }
}
