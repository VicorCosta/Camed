using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgenciaTipoDeAgenciaCommandHandler : HandlerBase, IRequestHandler<SalvarAgenciaTipoDeAgenciaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAgenciaTipoDeAgenciaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAgenciaTipoDeAgenciaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }
            else
            {
                var tipoAgencia = await uow.GetRepository<Domain.Entities.TipoDeAgencia>()
                                              .QueryFirstOrDefaultAsync(r => r.Id == request.TipoDeAgenciaId, includes: new[] { "GrupoAgencia" });
                int grupoAgenciaId = tipoAgencia.GrupoAgencia.Id;
                var existeGrupo = await uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>()
                                                        .QueryFirstOrDefaultAsync(r => r.Agencia.Id == request.AgenciaId && r.TipoDeAgencia.GrupoAgencia.Id == grupoAgenciaId);
                if (existeGrupo != null)
                {
                    throw new ApplicationException("Não é permitido associar agência a mais de um tipo do mesmo grupo.");

                } else
                {
                    if (request.Id == 0)
                    {

                        Domain.Entities.AgenciaTipoDeAgencia acao = new Domain.Entities.AgenciaTipoDeAgencia();
                        acao.Agencia = await uow.GetRepository<Agencia>().GetByIdAsync((int)request.AgenciaId);
                        acao.TipoDeAgencia = await uow.GetRepository<TipoDeAgencia>().GetByIdAsync((int)request.TipoDeAgenciaId);

                        await uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>().AddAsync(acao);
                    }
                    else
                    {
                        var ac = await uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>().QueryFirstOrDefaultAsync(r => r.Id == request.Id);

                        ac.Agencia = await uow.GetRepository<Agencia>().GetByIdAsync((int)request.AgenciaId);
                        ac.TipoDeAgencia = await uow.GetRepository<TipoDeAgencia>().GetByIdAsync((int)request.TipoDeAgenciaId);

                        uow.GetRepository<Domain.Entities.AgenciaTipoDeAgencia>().Update(ac);
                    }

                    await uow.CommitAsync();

                    return await Task.FromResult(result);
                }
            }
        }
    }
}
