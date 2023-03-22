using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Services;
using Camed.SSC.Application.Util;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.Flow
{
    public class FlowSolicitacaoCommandHandler : HandlerBase, IRequestHandler<FlowSolicitacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public FlowSolicitacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(FlowSolicitacaoCommand request, CancellationToken cancellationToken)
        {

            request.Solicitante_nome = request.Segurado_nome;
            request.Solicitante_email = request.Segurado_email;
            request.Solicitante_telefone_principal = request.Segurado_telefone_principal;
            request.Solicitante_telefone_celular = String.Empty;
            request.Solicitante_telefone_adicional = String.Empty;

            request.Operacaodefinanciamento = "0";
            request.Datafimvigencia = String.Empty;
            request.Dadosadicionais = String.Empty;

            string segurado_telefone_celular = String.Empty;
            string segurado_telefone_adicional = String.Empty;

            var entity = new Domain.Entities.Solicitacao();

            var agenciaCamed = uow.GetRepository<Agencia>().QueryFirstOrDefaultAsync(x => x.Nome.StartsWith("999")).Result;

            if (agenciaCamed == null)
            {
                result.Message = "Agência Camed não identificada.";
                return await Task.FromResult(result);
            }

            var canalDeDistribuicao = uow.GetRepository<CanalDeDistribuicao>().QueryFirstOrDefaultAsync(x => x.Id == 2).Result;

            if (canalDeDistribuicao == null)
            {
                result.Message = "Canal de Distribuição não identificado.";
                return await Task.FromResult(result);
            }

            var usuarioWeb = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(x => x.Login.Equals("USUARIOWEB")).Result;

            if (usuarioWeb == null)
            {
                result.Message = "Usuário web não identificado.";
                return await Task.FromResult(result);
            }

            entity.Agencia = agenciaCamed;
            entity.AgenciaConta = agenciaCamed;
            entity.CanalDeDistribuicao = canalDeDistribuicao;

            entity.Solicitante = new Domain.Entities.Solicitante();
            entity.Solicitante.Usuario = usuarioWeb;
            entity.Solicitante.Nome = request.Solicitante_nome;
            entity.Solicitante.Email = request.Solicitante_email;
            entity.Solicitante.TelefonePrincipal = request.Solicitante_telefone_principal;
            entity.Solicitante.TelefoneCelular = request.Solicitante_telefone_celular;
            entity.Solicitante.TelefoneAdicional = request.Solicitante_telefone_adicional;

            var vinculoBNB = uow.GetRepository<VinculoBNB>().QueryFirstOrDefaultAsync(x => x.Nome.Equals("Varejo")).Result;

            entity.Segurado = new Segurado();
            entity.Segurado.Nome = request.Segurado_nome;
            entity.Segurado.CpfCnpj = request.Segurado_cpfcnpj;
            entity.Segurado.Email = request.Segurado_email;
            entity.Segurado.TelefonePrincipal = request.Segurado_telefone_principal;
            entity.Segurado.TelefoneCelular = request.Segurado_telefone_celular;
            entity.Segurado.TelefoneAdicional = request.Segurado_telefone_adicional;
            entity.Segurado.VinculoBNB = vinculoBNB;

            if (string.IsNullOrEmpty(request.Dadosadicionais))
            {
                entity.DadosAdicionais = ".";
            }
            else
            {
                entity.DadosAdicionais = request.Dadosadicionais;
            }
            entity.TipoDeSeguro = new TipoDeSeguro();
            entity.TipoDeSeguro.Id = int.Parse(request.Tipodeseguro);
            entity.TipoDeProduto = new TipoDeProduto();
            entity.TipoDeProduto.Id = int.Parse(request.Tipodeproduto);
            entity.OperacaoDeFinanciamento = int.Parse(request.Operacaodefinanciamento);

            if (request.Datafimvigencia != "")
            {
                var aux = request.Datafimvigencia.Split('/');
                DateTime dtfimvigencia = new DateTime(int.Parse(aux[2]), int.Parse(aux[1]), int.Parse(aux[0]));
                entity.DataFimVigencia = dtfimvigencia;
            }
            else
            {
                if (request.Tipodeseguro == "8")
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
            entity.Origem = 3; //Web
            entity.Operador = usuarioWeb;
            entity.Aplicacao = "P";

            await uow.GetRepository<Domain.Entities.Solicitacao>().AddAndSaveAsync(entity);
            await uow.CommitAsync();
            
            new MailClient(uow).SendNewSolicitacaoPortal(request.Segurado_email, entity.Numero, request.Segurado_nome);

            result.Payload = entity.Numero;
            return await Task.FromResult(result);
        }
    }
}
