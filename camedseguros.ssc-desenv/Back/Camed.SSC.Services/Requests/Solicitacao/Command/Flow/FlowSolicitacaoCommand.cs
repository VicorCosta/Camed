using Camed.SSC.Application.Requests.Solicitacao.Validators.Flow;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.Flow
{
    public class FlowSolicitacaoCommand : CommandBase, IRequest<IResult>
    {
        public FlowSolicitacaoCommand()
        {
            this.Anexos = new HashSet<AnexoDeSolicitacao>();
            this.Acompanhamentos = new HashSet<Acompanhamento>();
            this.AgendamentosDeLigacao = new HashSet<AgendamentoDeLigacao>();
            this.CheckList = new HashSet<SolCheckList>();
            this.Indicacoes = new HashSet<SolicitacaoIndicacoes>();
            this.Checkins = new HashSet<Checkin>();
            this.Aplicacao = "W";
        }
        public int Id { get; set; }
        public int Numero { get; set; }
        public virtual int? Atendente_Id { get; set; }
        public virtual int Operador_Id { get; set; }
        public DateTime DataDeIngresso { get; set; }
        public virtual int Solicitante_Id { get; set; }
        public virtual int Agencia_Id { get; set; }
        public virtual int? Produtor_Id { get; set; }
        public virtual int TipoDeProduto_Id { get; set; }
        public virtual int? CanalDeDistribuicao_Id { get; set; }
        public virtual int TipoDeSeguro_Id { get; set; }
        public int? OperacaoDeFinanciamento { get; set; }
        public string DadosAdicionais { get; set; }
        public virtual int Segurado_Id { get; set; }
        public virtual int? Segmento_Id { get; set; }
        public virtual int AreaDeNegocio_Id { get; set; }
        public virtual ICollection<AnexoDeSolicitacao> Anexos { get; set; }
        public virtual ICollection<Acompanhamento> Acompanhamentos { get; set; }
        public virtual ICollection<AgendamentoDeLigacao> AgendamentosDeLigacao { get; set; }
        public virtual int SituacaoAtual_Id { get; set; }
        public DateTime DataHoraSituacaoAtual { get; set; }
        public int Origem { get; set; }
        public string CodigoDoBem { get; set; }
        public string NumeroFinanciamento { get; set; }
        public virtual int? Seguradora_Id { get; set; }
        public virtual int? Ramo_Id { get; set; }
        public string Nu_Proposta_Seguradora { get; set; }
        public virtual int? TipoSeguroGS_Id { get; set; }
        public string Nu_Apolice_Anterior { get; set; }
        public decimal? Pc_comissao { get; set; }

        public decimal? Co_Corretagem { get; set; }
        public decimal? Pc_agenciamento { get; set; }
        public decimal? VL_IS { get; set; }
        public virtual int? FL_Forma_Pagamento_1a_Id { get; set; }
        public virtual int? FL_Forma_Pagamento_Demais_Id { get; set; }
        public virtual int? GrupoDeProducao_Id { get; set; }
        public virtual int? TipoDeCategoria_Id { get; set; }
        public bool? Sede_Envia_Doc_Fisico { get; set; }
        public int? Nu_Sol_Vistoria { get; set; }
        public bool? Cadastrado_GS { get; set; }
        public decimal? Cd_estudo { get; set; }

        public decimal? estudo_origem { get; set; }
        public virtual ICollection<SolCheckList> CheckList { get; set; }
        public virtual ICollection<SolicitacaoIndicacoes> Indicacoes { get; set; }
        public virtual int? TipoDeCancelamento_Id { get; set; }
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
        public virtual int? AgenciaConta_Id { get; set; }
        public string TipoComissaoRV { get; set; }

        public virtual ICollection<Checkin> Checkins { get; set; }

        public string Aplicacao { get; set; }
        public virtual int? SeguradoraCotacao_Id { get; set; }
        public bool? Rastreador { get; set; }
        public DateTime? DataVencimento1aParc { get; set; }
        public decimal? vlr_premiotot_prop { get; set; }

        public string Solicitante_nome { get; set; }
        public string Solicitante_email { get; set; }
        public string Solicitante_telefone_principal { get; set; }
        public string Solicitante_telefone_celular { get; set; }
        public string Solicitante_telefone_adicional { get; set; }
        public string Tipodeseguro { get; set; }
        public string Datafimvigencia { get; set; }
        public string Dadosadicionais { get; set; }
        public string Tipodeproduto { get; set; }
        public string Segurado_nome { get; set; }
        public string Segurado_cpfcnpj { get; set; }
        public string Segurado_email { get; set; }
        public string Segurado_telefone_principal { get; set; }
        public string Segurado_telefone_celular { get; set; }
        public string Segurado_telefone_adicional { get; set; }
        public string Modelo { get; set; }
        public string Operacaodefinanciamento { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new FlowSolicitacaoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
