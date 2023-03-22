using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
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
    public class SalvarGrupoCommandHandler : HandlerBase, IRequestHandler<SalvarGrupoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarGrupoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarGrupoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Grupo acao = new Domain.Entities.Grupo();
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                acao.SempreVisualizarObservacao = request.SempreVisualizarObservacao;
                acao.AtribuirAtendente = request.AtribuirAtendente;
                acao.AtribuirOperador = request.AtribuirOperador;
                acao.CancelarSolicitacao = request.CancelarSolicitacao;

                if ( request.Menus != null )
                    acao.Menus.UpdateCollection(request.Menus.ToArray(), "Menu_Id");
                else acao.Menus = null;

                if ( request.Subgrupos != null ) 
                    acao.SubGrupos.UpdateCollection(request.Subgrupos.ToArray(), "Subgrupo_Id");
                 else
                    acao.SubGrupos = null;


                await uow.GetRepository<Domain.Entities.Grupo>().AddAsync(acao);
            } 
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Grupo>().QueryFirstOrDefaultAsync(r => r.Id == request.Id, includes: new[] { "Menus", "SubGrupos" });
                acao.Nome = request.Nome;
                acao.Ativo = request.Ativo;
                acao.SempreVisualizarObservacao = request.SempreVisualizarObservacao;
                acao.AtribuirAtendente = request.AtribuirAtendente;
                acao.AtribuirOperador = request.AtribuirOperador;
                acao.CancelarSolicitacao = request.CancelarSolicitacao;

                if ( request.Menus != null )
                    acao.Menus.UpdateCollection(request.Menus.ToArray(), "Menu_Id");
                else
                    acao.Menus = null;

                if ( request.Subgrupos != null )
                    acao.SubGrupos.UpdateCollection(request.Subgrupos.ToArray(), "Subgrupo_Id");
                else
                    acao.SubGrupos = null;

                uow.GetRepository<Domain.Entities.Grupo>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
