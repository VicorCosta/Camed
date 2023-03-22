using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Services;
using Camed.SSC.Application.Util;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.Salvar
{
    public class PostSolicitacaoCommandHandler : HandlerBase, IRequestHandler<PostSolicitacaoCommand, IResult>
    {
        readonly IUnitOfWork uow;

        public PostSolicitacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(PostSolicitacaoCommand request, CancellationToken cancellationToken)
        {
            string solicitante_nome = request.Solicitante_nome;
            string solicitante_email = request.Solicitante_email;
            string solicitante_telefone_principal = request.Solicitante_telefone_principal;
            string solicitante_telefone_celular = request.Solicitante_telefone_celular;
            string solicitante_telefone_adicional = request.Solicitante_telefone_adicional;
            string tipodeseguro = request.Tipodeseguro;
            string operacaodefinanciamento = "0";
            string datafimvigencia = request.Datafimvigencia;
            string dadosadicionais = request.Dadosadicionais;
            string tipodeproduto = request.Tipodeproduto;
            string segurado_nome = request.Segurado_nome;
            string segurado_cpfcnpj = request.Segurado_cpfcnpj;
            string segurado_email = request.Segurado_email;
            string segurado_telefone_principal = request.Segurado_telefone_principal;
            string segurado_telefone_celular = request.Segurado_telefone_celular;
            string segurado_telefone_adicional = request.Segurado_telefone_adicional;

            var formatter = request.Modelo;

            var entity = new Domain.Entities.Solicitacao();

            var agenciaCamed = uow.GetRepository<Agencia>().QueryFirstOrDefaultAsync(w => w.Nome.StartsWith("999")).Result;

            if (agenciaCamed == null)
            {
                throw new ApplicationException("Agência Camed não identificada.");
            }

            var canalDeDistribuicao = uow.GetRepository<CanalDeDistribuicao>().QueryFirstOrDefaultAsync(x => x.Id == 2).Result;

            if (canalDeDistribuicao == null)
            {
                throw new ApplicationException("Canal de Distribuição não identificado.");
            }

            var usuarioWeb = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(x => x.Login.Equals("USUARIOWEB")).Result;

            if (usuarioWeb == null)
            {
                throw new ApplicationException("Usuário web não identificado.");
            }

            entity.Agencia = agenciaCamed;
            entity.AgenciaConta = agenciaCamed;
            entity.CanalDeDistribuicao = canalDeDistribuicao;

            entity.Solicitante = new Domain.Entities.Solicitante();
            entity.Solicitante.Usuario = usuarioWeb;
            entity.Solicitante.Nome = solicitante_nome;
            entity.Solicitante.Email = solicitante_email;
            entity.Solicitante.TelefonePrincipal = solicitante_telefone_principal;
            entity.Solicitante.TelefoneCelular = solicitante_telefone_celular;
            entity.Solicitante.TelefoneAdicional = solicitante_telefone_adicional;

            var vinculoBNB = uow.GetRepository<VinculoBNB>().QueryFirstOrDefaultAsync(x => x.Nome.Equals("Varejo")).Result;

            entity.Segurado = new Segurado();
            entity.Segurado.Nome = segurado_nome;
            entity.Segurado.CpfCnpj = segurado_cpfcnpj;
            entity.Segurado.Email = segurado_email;
            entity.Segurado.TelefonePrincipal = segurado_telefone_principal;
            entity.Segurado.TelefoneCelular = segurado_telefone_celular;
            entity.Segurado.TelefoneAdicional = segurado_telefone_adicional;
            entity.Segurado.VinculoBNB = vinculoBNB;

            if (string.IsNullOrEmpty(dadosadicionais))
            {
                entity.DadosAdicionais = ".";
            }
            else
            {
                entity.DadosAdicionais = dadosadicionais;
            }
            entity.TipoDeSeguro = new TipoDeSeguro();
            entity.TipoDeSeguro.Id = int.Parse(tipodeseguro);
            entity.TipoDeProduto = new TipoDeProduto();
            entity.TipoDeProduto.Id = int.Parse(tipodeproduto);
            entity.OperacaoDeFinanciamento = int.Parse(operacaodefinanciamento);

            if (datafimvigencia != "")
            {
                var aux = datafimvigencia.Split('/');
                DateTime dtfimvigencia = new DateTime(int.Parse(aux[2]), int.Parse(aux[1]), int.Parse(aux[0]));
                entity.DataFimVigencia = dtfimvigencia;
            }
            else
            {
                if (tipodeseguro == "8")
                {
                    DateTime dtfimvigencia = DateTime.Now;
                    entity.DataFimVigencia = dtfimvigencia;
                }
                else
                {
                    entity.DataFimVigencia = null;
                }
            };

            SequencialDeSolicitacao ultimoId = new SequencialDeSolicitacao();
            ultimoId.Operador = usuarioWeb;
            ultimoId.Data = DateTime.Now;

            await uow.GetRepository<SequencialDeSolicitacao>().AddAndSaveAsync(ultimoId);

            await uow.CommitAsync();

            entity.Numero = ultimoId.Id;
            entity.Origem = 3;
            entity.Operador = usuarioWeb;
            entity.Aplicacao = "P";

            await uow.GetRepository<Domain.Entities.Solicitacao>().AddAndSaveAsync(entity);

            new MailClient(uow).SendNewSolicitacaoPortal(segurado_email, entity.Numero, segurado_nome);

            await uow.CommitAsync();
            result.Payload = entity.Numero;
            return await Task.FromResult(result);
        }
    }
}
