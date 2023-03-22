using Camed.SSC.Application.Requests.Solicitacao.Validators.SolicitacaoBNB;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SolicitacaoBNB
{
    public class SolicitacaoBNBCommand : CommandBase, IRequest<IResult>
    {
        public string Matricula { get; set; }
        public string CodigoAgencia { get; set; }
        public int OperacaoDeFinanciamento { get; set; }
        public string DadosAdicionais { get; set; }
        public int Segmento_Id { get; set; }
        public int TipoDeSeguro_Id { get; set; }
        public int CanalDeDistribuicao_Id { get; set; }
        public int TipoDeProduto_Id { get; set; }
        public Segurado Segurado { get; set; }
        public string NumeroFinanciamento { get; set; }
        public int OrcamentoPrevio { get; set; }
        public string CodigoDoBem { get; set; }
        public string TelefonePrincipal { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneAdicional { get; set; }
        public ICollection<AnexoBNB> Anexos { get; set; }
        public ICollection<Acompanhamento> Acompanhamentos { get; set; }
        public int SituacaoAtual_Id { get; set; }
        public DateTime DataHoraSituacaoAtual { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SolicitacaoBNBValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class AnexoBNB
    {
        public string NomeDoArquivo { get; set; }
        public string ConteudoBase64 { get; set; }
    }
}
