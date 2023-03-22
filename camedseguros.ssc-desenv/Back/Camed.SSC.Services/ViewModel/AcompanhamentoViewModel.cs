using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.ViewModel
{
    public class AcompanhamentoViewModel
    {
        public int Solicitacao_Id { get; set; }
        public Commom Acao { get; set; }
        public string Observacao { get; set; }
        public bool PermiteVisualizarObservacao { get; set; }
        public bool PermiteVisualizarAnexo { get; set; }
        public string CodigoDoBem { get; set; }
        public string NumeroFinanciamento { get; set; }
        public bool Cadastrado_GS { get; set; }
        public string Sede_Envia_Doc_Fisico { get; set; }
        public Commom Produtor { get; set; }
        public Commom Ramo { get; set; }
        public Commom Segmento { get; set; }
        public Commom Seguradora { get; set; }
        public decimal? Cd_estudo { get; set; }
        public string Nu_Apolice_Anterior { get; set; }
        public decimal? Pc_agenciamento { get; set; }
        public decimal? Pc_comissao { get; set; }
        public decimal? Co_Corretagem { get; set; }
        public decimal? VL_IS { get; set; }
        public int? TipoSeguroGS { get; set; }
        public int? FL_Forma_Pagamento_1a { get; set; }
        public int? FL_Forma_Pagamento_Demais { get; set; }
        public int? GrupoDeProducao { get; set; }
        public int? TipoDeCategoria { get; set; }
        public int? TipoDeProduto_Id { get; set; }
        public int? Nu_Sol_Vistoria { get; set; }
        public string Nu_Proposta_Seguradora { get; set; }
        public int? CanalDeDistribuicao_Id { get; set; }
        public bool eVendaComp { get; set; }
        public VendaCompartilhada[] dadosVendaComp { get; set; }
        public List<AnexoDeAcompanhamento> Anexos { get; set; }
        public string TipoEndosso { get; set; }
        public int? MotivoEndossoCancelamento { get; set; }
        public int? MotivoRecusa { get; set; }
        public string VIP { get; set; }
        public string CROSSUP { get; set; }
        public bool Rechaco { get; set; }
        public decimal? vlr_premiotot_atual { get; set; }
        public decimal? perc_comissao_atual { get; set; }
        public string VistoriaNec { get; set; }
        public string ObsVistoria { get; set; }
        public string TipoComissaoRV { get; set; }
        public string Rastreador { get; set; }
        public DateTime? DataVencimento1aParc { get; set; }
        public Commom SeguradoraCotacao { get; set; }
        public decimal? vlr_premiotot_prop { get; set; }
        public string Email { get; set; }
        public string EmailSecundario { get; set; }
        public string TelefoneAdicional { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefonePrincipal { get; set; }
        public string PermiteEmailAoSegurado { get; set; }

        public class Commom
        {
            public int Id { get; set; }
        }

        public class VendaCompartilhada
        {
            public Commom Produtor { get; set; }
            public decimal Percentual { get; set; }
        }
    }
}
