using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests {
    public class SalvarTipoDeDocumentoCommandHandler : HandlerBase, IRequestHandler<SalvarTipoDeDocumentoCommand, IResult> {
        private readonly IUnitOfWork uow;

        public SalvarTipoDeDocumentoCommandHandler(IUnitOfWork uow) {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTipoDeDocumentoCommand request, CancellationToken cancellationToken) {
            try
            {
                if (!request.IsValid())
                {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                if (request.Ordem <= 0)
                {
                    throw new ApplicationException("Número da ordem inválido. Verifique e tente novamente.");
                }

                if (request.Id == 0)
                {
                    Domain.Entities.TipoDeDocumento tipoDeDocumento = new Domain.Entities.TipoDeDocumento();

                    tipoDeDocumento.Nome = request.Nome;

                    if (request.RamosDeSeguro != null)
                    {
                        foreach (var produto in request.RamosDeSeguro)
                        {
                            tipoDeDocumento.TiposDeProdutoMorte.Add(new Domain.Entities.TipoDeDocumentoTipoDeProdutoTipoMorte
                            {
                                TipoDeProduto_Id = produto,
                                TipoMorte_Id = request.TipoMorte_id,
                                Ordem = request.Ordem,
                                Obrigatorio = request.Obrigatorio,
                                Ativo = request.Ativo //Não tem essa coluna no DB.
                            });
                        }
                    }

                    await uow.GetRepository<Domain.Entities.TipoDeDocumento>().AddAsync(tipoDeDocumento);

                    await uow.CommitAsync();
                }
                else
                {

                    var tipoDeDocumento = await uow.GetRepository<Domain.Entities.TipoDeDocumento>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "TiposDeProduto", "TiposDeProduto.TipoDeProduto", "TiposDeProdutoMorte" });

                    tipoDeDocumento.Nome = request.Nome;

                    if (request.RamosDeSeguro != null)
                    {
                        var _tipoMorte = uow.GetRepository<Domain.Entities.TipoMorte>().GetByIdAsync(request.TipoMorte_id.Value).Result;

                        tipoDeDocumento.TiposDeProdutoMorte.UpdateCollection(request.RamosDeSeguro, keyProperty: "TipoDeProduto_Id");

                        foreach (var td in tipoDeDocumento.TiposDeProdutoMorte)
                        {
                            td.TipoMorte = _tipoMorte;
                            td.TipoMorte_Id = request.TipoMorte_id;
                            td.Ativo = request.Ativo; // Não existe essa coluna no DB
                            td.Ordem = request.Ordem;
                            td.Obrigatorio = request.Obrigatorio;
                        }
                    }

                    if (!request.Ativo && !request.Obrigatorio && request.Ordem == null && request.RamosDeSeguro == null && request.TipoMorte_id == null)
                    {
                        tipoDeDocumento.TiposDeProdutoMorte.UpdateCollection(request.RamosDeSeguro, keyProperty: "TipoDeProduto_Id");
                    }

                    uow.GetRepository<Domain.Entities.TipoDeDocumento>().Update(tipoDeDocumento);

                    await uow.CommitAsync();
                }
            } catch (Exception e){
                throw new ApplicationException("" + e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
