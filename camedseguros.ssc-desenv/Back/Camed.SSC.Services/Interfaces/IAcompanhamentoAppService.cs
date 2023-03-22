using Camed.SSC.Application.ViewModel;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Interfaces
{
    public interface IAcompanhamentoAppService
    {
        Acompanhamento ObterUltimoAcompanhamento(Solicitacao solicitacao);
        void AdicionarAcompanhamento(AcompanhamentoViewModel model, string login = null);
        void Cadastrar(int idDaSolicitacao, int idDaAcao, List<Grupo> gruposDoUsuario, string observacao, bool permiteVisualizarObservacao, List<AnexoDeAcompanhamento> anexos,
                      dynamic usuarioAtual, string codigoDoBem, string numeroFinanciamento, int? seguradora, int? ramo, string nu_proposta_seguradora, int? tipoSeguroGS,
                      string nu_apolice_anterior, decimal? pc_comissao, decimal? co_corretagem, decimal? pc_agenciamento, decimal? vl_is, int? fl_forma_pagamento_1a, int? fl_forma_pagamento_demais,
                      int? grupodeproducao, int? tipoDeCategoriaId, int? tipoDeProduto_Id, bool? sede_envia_doc_fisico, int? nu_sol_vistoria, bool cadastrado_gs, decimal? cd_estudo, int? segmento,
                      int? produtor, List<VendaCompartilhada> vendaCompartilhada, string tipoEndosso, int? motivoEndossoCancelamento, int? motivoRecusa, bool? vip, bool? crossup, DateTime agora,
                      ref bool enviaSMSsegurado, bool rechaco, decimal? vlr_premiotot_atual, decimal? perc_comissao_atual, bool? VistoriaNec, string ObsVistoria, string TipoComissaoRV, int? produtorId,
                      int? seguradoracotacao, decimal? vlr_premiotot_prop, bool permiteVisualizarAnexo, bool? Rastreador, DateTime? DataVencimento1aParc, bool? permiteEmailAoSegurado);
    }
}
