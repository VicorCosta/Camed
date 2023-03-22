using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirAgenciaTipoDeAgenciaCommandHandler : HandlerBase, IRequestHandler<ExcluirAgenciaTipoDeAgenciaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirAgenciaTipoDeAgenciaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirAgenciaTipoDeAgenciaCommand request, CancellationToken cancellationToken)
        {

            var acao = await uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>().QueryFirstOrDefaultAsync(r=>r.Id== request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Relação não localizada na base de dados.";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
