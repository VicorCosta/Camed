using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Services
{
    public class AreaDeNegocioAppService : IAreaDeNegocioAppService
    {
        private readonly IUnitOfWork uow;

        public AreaDeNegocioAppService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public AreaDeNegocio RetornarAreaNegocio(Solicitacao solicitacao)
        {
            int tipoDeSeguroAux = solicitacao.TipoDeSeguro.Id;
            if (solicitacao.TipoDeSeguro.Id == 6)
                solicitacao.TipoDeSeguro.Id = 1;

            VinculoBNB vinculoBNB = solicitacao.Segurado.VinculoBNB;
            int? vinculoBNBId = (vinculoBNB == null) ? (int?)null : vinculoBNB.Id;
            int? operacaoDeFinanciamento_Id = solicitacao.OperacaoDeFinanciamento;

            var grupoAgencia_Id = uow.GetRepository<TipoDeSeguro>().QueryFirstOrDefaultAsync(x => x.Id == solicitacao.TipoDeSeguro.Id,
                includes: new[] { "GrupoAgencia" }).Result.GrupoAgencia.Id;

            var ata = uow.GetRepository<AgenciaTipoDeAgencia>().QueryFirstOrDefaultAsync(x => x.Agencia.Id == solicitacao.Agencia.Id
                    && x.TipoDeAgencia.GrupoAgencia.Id == grupoAgencia_Id, includes: new[] { "Agencia", "TipoDeAgencia.GrupoAgencia" }).Result;

            if (ata == null)
            {
                solicitacao.TipoDeSeguro.Id = tipoDeSeguroAux;
                return uow.GetRepository<AreaDeNegocio>().QueryFirstOrDefaultAsync(x => x.Nome.ToUpper().Equals("INDEFINIDA")).Result;
            }
            else
            {
                var mapeamentoAreaDeNegocio = uow.GetRepository<MapeamentoAreaDeNegocio>().QueryFirstOrDefaultAsync(x => x.TipoDeAgencia.Id == ata.TipoDeAgencia.Id
                        && x.TipoDeProduto.Id == solicitacao.TipoDeProduto.Id
                        && x.TipoDeSeguro.Id == solicitacao.TipoDeSeguro.Id
                        && (vinculoBNBId == null ? x.VinculoBNB == null : x.VinculoBNB.Id == vinculoBNBId)
                        && (operacaoDeFinanciamento_Id == null ? x.OperacaoDeFinanciamento == null :
                        x.OperacaoDeFinanciamento == operacaoDeFinanciamento_Id), includes: new[] { "AreaDeNegocio" }).Result;

                solicitacao.TipoDeSeguro.Id = tipoDeSeguroAux;

                if (mapeamentoAreaDeNegocio == null || mapeamentoAreaDeNegocio.AreaDeNegocio == null)
                    return uow.GetRepository<AreaDeNegocio>().QueryFirstOrDefaultAsync(x => x.Nome.ToUpper().Equals("INDEFINIDA")).Result;
                else
                    return mapeamentoAreaDeNegocio.AreaDeNegocio;
            }
        }
    }
}
