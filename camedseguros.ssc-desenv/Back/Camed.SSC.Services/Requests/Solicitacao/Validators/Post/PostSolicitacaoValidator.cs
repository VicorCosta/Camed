using Camed.SSC.Application.Requests.Solicitacao.Command.Salvar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.Salvar
{
    public class PostSolicitacaoValidator : AbstractValidator<PostSolicitacaoCommand>
    {
        public PostSolicitacaoValidator()
        {
            ValidateSolicitacao();
        }

        private void ValidateSolicitacao()
        {
            RuleFor(p => p.Numero)
                .NotEmpty().WithMessage("É preciso informar o número")
                .NotNull().WithMessage("É preciso informar o número")
                .NotEqual(0).WithMessage("É preciso informar o número");

            RuleFor(p => p.Operador_Id)
                .NotEmpty().WithMessage("É obrigatório informar o Operador")
                .NotNull().WithMessage("É obrigatório informar o Operador")
                .NotEqual(0).WithMessage("É obrigatório informar o Operador");

            RuleFor(p => p.DataDeIngresso)
                .NotEmpty().WithMessage("É necessário informar a Data de Ingresso")
                .NotNull().WithMessage("É necessário informar a Data de Ingresso");

            RuleFor(p => p.Solicitante_Id)
                .NotEmpty().WithMessage("É obrigatório informar o Solicitante")
                .NotNull().WithMessage("É obrigatório informar o Solicitante")
                .NotEqual(0).WithMessage("É obrigatório informar o Solicitante");

            RuleFor(p => p.Agencia_Id)
                .NotEmpty().WithMessage("É obrigatório informar a Agência")
                .NotNull().WithMessage("É obrigatório informar a Agência")
                .NotEqual(0).WithMessage("É obrigatório informar a Agência");

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

            RuleFor(p => p.Segurado_Id)
                .NotEmpty().WithMessage("É obrigatório informar o Segurado")
                .NotNull().WithMessage("É obrigatório informar o Segurado")
                .NotEqual(0).WithMessage("É obrigatório informar o Segurado");

            RuleFor(p => p.AreaDeNegocio_Id)
                .NotEmpty().WithMessage("É obrigatório informar a Area De Negocio")
                .NotNull().WithMessage("É obrigatório informar a Area De Negocio")
                .NotEqual(0).WithMessage("É obrigatório informar a AreaDeNegocio");

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
