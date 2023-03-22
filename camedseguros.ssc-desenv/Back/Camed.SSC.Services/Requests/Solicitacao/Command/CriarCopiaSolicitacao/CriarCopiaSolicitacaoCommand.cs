using Camed.SSC.Application.Requests.Solicitacao.Validators.CriarCopiaSolicitacao;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.CriarCopiaSolicitacao
{
    public class CriarCopiaSolicitacaoCommand : CommandBase, IRequest<IResult>
    {
        public CriarCopiaSolicitacaoCommand()
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
        public int Atendente_Id { get; set; }
        public int Operador_Id { get; set; }
        public DateTime DataDeIngresso { get; set; }
        public int Solicitante_Id { get; set; }
        public int Agencia_Id { get; set; }
        public int Produtor_Id { get; set; }
        public int TipoDeProduto_Id { get; set; }
        public int CanalDeDistribuicao_Id { get; set; }
        public int TipoDeSeguro_Id { get; set; }
        public int? OperacaoDeFinanciamento { get; set; }
        public string DadosAdicionais { get; set; }
        public int Segurado_Id { get; set; }
        public int Segmento_Id { get; set; }
        public int AreaDeNegocio_Id { get; set; }
        public virtual ICollection<AnexoDeSolicitacao> Anexos { get; set; }
        public virtual ICollection<Acompanhamento> Acompanhamentos { get; set; }
        public virtual ICollection<AgendamentoDeLigacao> AgendamentosDeLigacao { get; set; }
        public int SituacaoAtual_Id { get; set; }
        public DateTime DataHoraSituacaoAtual { get; set; }
        public int Origem { get; set; }
        public string CodigoDoBem { get; set; }
        public string NumeroFinanciamento { get; set; }
        public int Seguradora_Id { get; set; }
        public int Ramo_Id { get; set; }
        public string Nu_Proposta_Seguradora { get; set; }
        public int TipoSeguroGS_Id { get; set; }
        public string Nu_Apolice_Anterior { get; set; }
        public decimal? Pc_comissao { get; set; }
        public decimal? Co_Corretagem { get; set; }
        public decimal? Pc_agenciamento { get; set; }
        public decimal? VL_IS { get; set; }
        public int FL_Forma_Pagamento_1a_Id { get; set; }
        public int FL_Forma_Pagamento_Demais_Id { get; set; }
        public int GrupoDeProducao_Id { get; set; }
        public int TipoDeCategoria_Id { get; set; }
        public bool? Sede_Envia_Doc_Fisico { get; set; }
        public int? Nu_Sol_Vistoria { get; set; }
        public bool? Cadastrado_GS { get; set; }
        public decimal? Cd_estudo { get; set; }
        public decimal? estudo_origem { get; set; }
        public virtual ICollection<SolCheckList> CheckList { get; set; }
        public virtual ICollection<SolicitacaoIndicacoes> Indicacoes { get; set; }
        public int TipoDeCancelamento_Id { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public int? QtdDiasSLARenovacao { get; set; }
        public string TipoEndosso { get; set; }
        public int MotivoEndossoCancelamento_Id { get; set; }
        public int MotivoRecusa_Id { get; set; }
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
        public int AgenciaConta_Id { get; set; }
        public string TipoComissaoRV { get; set; }

        public virtual ICollection<Checkin> Checkins { get; set; }

        public string Aplicacao { get; set; }
        public int SeguradoraCotacao_Id { get; set; }
        public bool? Rastreador { get; set; }
        public DateTime? DataVencimento1aParc { get; set; }
        public decimal? vlr_premiotot_prop { get; set; }
        public int Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new CriarCopiaSolicitacaoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
