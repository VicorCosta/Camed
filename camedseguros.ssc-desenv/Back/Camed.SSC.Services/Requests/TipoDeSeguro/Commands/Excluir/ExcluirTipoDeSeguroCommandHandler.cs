using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirTipoDeSeguroCommandHandler : HandlerBase, IRequestHandler<ExcluirTipoDeSeguroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirTipoDeSeguroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirTipoDeSeguroCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.TipoDeSeguro>()
                .QueryFirstOrDefaultAsync(r=>r.Id == request.Id, includes: new[] { "Empresas", "Empresas.Empresa", "TiposDeProduto", "TiposDeProduto.TipoDeProduto"});

            if (acao != null)
            {
                if (acao.Empresas.Any())
                {
                    var lista = acao.Empresas.Select(s => s.Empresa.Nome);
                    throw new ApplicationException($"Contém uma relação com a(s) empresa(s): '{ string.Join(", ", lista) }' ");
                }
                await uow.GetRepository<Domain.Entities.TipoDeSeguro>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Tipo de Seguro, não localizada na base de dados";
                return await Task.FromResult(result);
            }

            
            return await Task.FromResult(result);
        }

    }
}
