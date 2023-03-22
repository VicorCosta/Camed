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

    public class ExcluirGrupoAgenciaCommandHandler : HandlerBase, IRequestHandler<ExcluirGrupoAgenciaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirGrupoAgenciaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirGrupoAgenciaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.GrupoAgencia>().GetByIdAsync(request.Id);

            var tipoDeSeguro = await uow.GetRepository<Domain.Entities.TipoDeSeguro>().QueryAsync(r=>r.GrupoAgencia.Id == acao.Id,includes: new[] { "GrupoAgencia" });
            


            if (acao != null)
            {
                if (tipoDeSeguro.Any())
                {
                     var s = tipoDeSeguro.Select(r=>r.Nome);
                     throw new ApplicationException($"Grupo de agência contém relação com o(s) tipo(s) de seguro {string.Join(", ", s)}");
                }
                var tipoDeAgencias = await uow.GetRepository<TipoDeAgencia>().QueryAsync(r=>r.GrupoAgencia.Id == acao.Id, includes:new[] { "GrupoAgencia" });
                
                if (tipoDeAgencias.Any())
                {
                    var s = tipoDeAgencias.Select(r => r.Nome);
                    throw new ApplicationException($"Grupo de agência contém relação com o(s) tipo(s) de Agência {string.Join(", ", s)}");
                }
                var tipoDeCancelamento = await uow.GetRepository<TipoDeCancelamento>().QueryFirstOrDefaultAsync(r => r.GrupoAgencia.Id == acao.Id);
                if (tipoDeCancelamento != null )
                {
                    var s = tipoDeCancelamento.Descricao;
                    throw new ApplicationException($"Grupo de agência contém relação com o(s) tipo(s) de Agência {s}");
                }
                await uow.GetRepository<Domain.Entities.GrupoAgencia>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Grupo Agencia não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
