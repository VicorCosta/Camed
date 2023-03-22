using Camed.SSC.Application.Requests.Solicitacao.Command.SolicitacaoBNB;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.SolicitacaoBNB
{
    public class SolicitacaoBNBValidator : AbstractValidator<SolicitacaoBNBCommand>
    {
        public SolicitacaoBNBValidator()
        {
            ValidateSolicitacaoBNB();
        }

        private void ValidateSolicitacaoBNB()
        {

            RuleFor(p => p.TipoDeProduto_Id)
                .NotEmpty().WithMessage("É obrigatório informar o Tipo de Produto")
                .NotNull().WithMessage("É obrigatório informar o Tipo de Produto")
                .NotEqual(0).WithMessage("É obrigatório informar o Tipo de Produto");

            RuleFor(p => p.TipoDeSeguro_Id)
                .NotEmpty().WithMessage("É obrigatório informar o Tipo de Seguro")
                .NotNull().WithMessage("É obrigatório informar o Tipo de Seguro")
                .NotEqual(0).WithMessage("É obrigatório informar o Tipo de Seguro");

            RuleFor(p => p.DadosAdicionais)
                .NotEmpty().WithMessage("É necessário informar Dados Adicionais")
                .NotNull().WithMessage("É necessário informar Dados Adicionais");

            RuleFor(p => p.SituacaoAtual_Id)
                .NotEmpty().WithMessage("É obrigatório informar a Situacao Atual")
                .NotNull().WithMessage("É obrigatório informar a Situacao Atual")
                .NotEqual(0).WithMessage("É obrigatório informar a Situacao Atual");

            RuleFor(p => p.DataHoraSituacaoAtual)
                .NotEmpty().WithMessage("É necessário informar a Data e Hora da Situação Atual")
                .NotNull().WithMessage("É necessário informar a Data  e Hora da Situação Atual");
        }
    }
}
