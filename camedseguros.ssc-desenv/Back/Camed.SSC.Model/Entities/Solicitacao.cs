using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Solicitacao : EntityBase
    {
        public Solicitacao()
        {
            this.Anexos = new HashSet<AnexoDeSolicitacao>();
            this.Acompanhamentos = new HashSet<Acompanhamento>();
            this.AgendamentosDeLigacao = new HashSet<AgendamentoDeLigacao>();
            this.CheckList = new HashSet<SolCheckList>();
            this.Indicacoes = new HashSet<SolicitacaoIndicacoes>();
            this.Checkins = new HashSet<Checkin>();
            this.Aplicacao = "W";
        }

        public int Numero { get; set; }
        public virtual Usuario Atendente { get; set; }
        public int Atendente_Id { get; set; }
        public virtual Usuario Operador { get; set; }
        public DateTime DataDeIngresso { get; set; }
        public virtual Solicitante Solicitante { get; set; }
        public virtual ICollection<AvAtendimento> Avaliacoes { get; set; }
        public virtual Agencia Agencia { get; set; }
        public virtual Funcionario Produtor { get; set; }
        public virtual TipoDeProduto TipoDeProduto { get; set; }
        public virtual CanalDeDistribuicao CanalDeDistribuicao { get; set; }
        public virtual TipoDeSeguro TipoDeSeguro { get; set; }
        public int? OperacaoDeFinanciamento { get; set; }
        public string DadosAdicionais { get; set; }
        public virtual Segurado Segurado { get; set; }
        public virtual Segmento Segmento { get; set; }
        public virtual AreaDeNegocio AreaDeNegocio { get; set; }
        public virtual ICollection<AnexoDeSolicitacao> Anexos { get; set; }
        public virtual ICollection<Acompanhamento> Acompanhamentos { get; set; }
        public virtual ICollection<AgendamentoDeLigacao> AgendamentosDeLigacao { get; set; }
       
        public int SituacaoAtual_Id { get; set; }
        public virtual Situacao SituacaoAtual { get; set; }
       
        public DateTime DataHoraSituacaoAtual { get; set; }
        public int Origem { get; set; }
        public string CodigoDoBem { get; set; }
        public string NumeroFinanciamento { get; set; }
        public virtual VW_SEGURADORA Seguradora { get; set; }
        public virtual Ramo Ramo { get; set; }
        public string Nu_Proposta_Seguradora { get; set; }
        public virtual TipoDeSeguroGS TipoSeguroGS { get; set; }
        public string Nu_Apolice_Anterior { get; set; }
        public decimal? Pc_comissao { get; set; }
        public decimal? Co_Corretagem { get; set; }
        public decimal? Pc_agenciamento { get; set; }
        public decimal? VL_IS { get; set; }
        public virtual FormaDePagamento FL_Forma_Pagamento_1a { get; set; }
        public virtual FormaDePagamento FL_Forma_Pagamento_Demais { get; set; }
        public virtual GrupoDeProducao GrupoDeProducao { get; set; }
        public virtual TipoDeCategoria TipoDeCategoria { get; set; }
        public bool? Sede_Envia_Doc_Fisico { get; set; }
        public int? Nu_Sol_Vistoria { get; set; }
        public bool? Cadastrado_GS { get; set; }
        public decimal? Cd_estudo { get; set; }
        public decimal? estudo_origem { get; set; }
        public virtual ICollection<SolCheckList> CheckList { get; set; }
        public virtual ICollection<SolicitacaoIndicacoes> Indicacoes { get; set; }
        public virtual TipoDeCancelamento TipoDeCancelamento { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public int? QtdDiasSLARenovacao { get; set; }
        public string TipoEndosso { get; set; }
        public MotivoEndossoCancelamento MotivoEndossoCancelamento { get; set; }
        public MotivoRecusa MotivoRecusa { get; set; }
        public bool? VIP { get; set; }
        public int? OrcamentoPrevio { get; set; }
        public bool? CROSSUP { get; set; }
        public int? Mercado { get; set; }
        public bool? Rechaco { get; set; }
        public decimal? vlr_premiotot_anterior { get; set; }
        public decimal? perc_comissao_anterior { get; set; }
        public decimal? vlr_premiotot_atual { get; set; }
        public decimal? perc_comissao_atual { get; set; }
        public bool? VistoriaNec { get; set; }
        public string ObsVistoria { get; set; }
        public virtual Agencia AgenciaConta { get; set; }
        public string TipoComissaoRV { get; set; }

        public virtual ICollection<Checkin> Checkins { get; set; }

        public string Aplicacao { get; set; }
        public virtual VW_SEGURADORA SeguradoraCotacao { get; set; }
        public bool? Rastreador { get; set; }
        public DateTime? DataVencimento1aParc { get; set; }
        public decimal? vlr_premiotot_prop { get; set; }
        public int? CotacaoSombrero_Id { get; set; }
        public CotacaoSombrero CotacaoSombrero { get; set; }
    }
}
