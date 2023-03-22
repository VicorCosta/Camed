using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SolicitacaoBNB
{
    public class SolicitacaoBNBCommandHandler : HandlerBase, IRequestHandler<SolicitacaoBNBCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identity;
        private readonly ISolicitacaoAppService solicitacaoAppService;

        public SolicitacaoBNBCommandHandler(IUnitOfWork uow, IIdentityAppService identity, ISolicitacaoAppService solicitacaoAppService)
        {
            this.uow = uow;
            this.identity = identity;
            this.solicitacaoAppService = solicitacaoAppService;
        }

        public async Task<IResult> Handle(SolicitacaoBNBCommand request, CancellationToken cancellationToken)
        {
            var usuarioLogado = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == identity.Identity.Id, includes: new[] { "Grupos", "AreasDeNegocio" }).Result;

            if (usuarioLogado.PermitidoGerarCotacao)
            {
                var sequencial = new SequencialDeSolicitacao();
                sequencial.Operador = usuarioLogado;
                sequencial.Data = DateTime.Now;

                await uow.GetRepository<SequencialDeSolicitacao>().AddAndSaveAsync(sequencial);

                var solicitante = new Domain.Entities.Solicitante();
                solicitante.Usuario = usuarioLogado;
                solicitante.Nome = usuarioLogado.Nome;
                solicitante.Email = usuarioLogado.Email;
                solicitante.TelefonePrincipal = request.TelefonePrincipal;
                solicitante.TelefoneCelular = request.TelefoneCelular;
                solicitante.TelefoneAdicional = request.TelefoneAdicional;

                Agencia agencia = uow.GetRepository<Agencia>().QueryAsync(x => x.Codigo.Equals(request.CodigoAgencia)).Result.FirstOrDefault();

                TipoDeSeguro tipoDeSeguro = uow.GetRepository<TipoDeSeguro>().QueryFirstOrDefaultAsync(x => x.Id == request.TipoDeSeguro_Id).Result;

                TipoDeProduto tipoDeProduto = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(x => x.Id == request.TipoDeProduto_Id).Result;

                Domain.Entities.Segmento segmento = uow.GetRepository<Domain.Entities.Segmento>().QueryFirstOrDefaultAsync(x => x.Id == request.Segmento_Id).Result;

                VinculoBNB vinculoBNB = uow.GetRepository<VinculoBNB>().QueryFirstOrDefaultAsync(x => x.Id == request.Segurado.VinculoBNB.Id).Result;

                CanalDeDistribuicao canalDeDistribuicao = uow.GetRepository<CanalDeDistribuicao>().QueryFirstOrDefaultAsync(
                    x => x.Id == request.CanalDeDistribuicao_Id).Result;

                var solicitacao = new Domain.Entities.Solicitacao();

                solicitacao.Solicitante = solicitante;
                solicitacao.Operador = usuarioLogado;
                solicitacao.Agencia = agencia;
                solicitacao.AgenciaConta = agencia;
                solicitacao.DataDeIngresso = DateTime.Now;
                solicitacao.OperacaoDeFinanciamento = request.OperacaoDeFinanciamento;
                solicitacao.DadosAdicionais = request.DadosAdicionais;
                solicitacao.Segmento = segmento;
                solicitacao.TipoDeSeguro = tipoDeSeguro;
                solicitacao.CanalDeDistribuicao = canalDeDistribuicao;
                solicitacao.TipoDeProduto = tipoDeProduto;
                solicitacao.NumeroFinanciamento = request.NumeroFinanciamento;
                solicitacao.OrcamentoPrevio = request.OrcamentoPrevio;
                solicitacao.Segurado = request.Segurado;
                solicitacao.Segurado.VinculoBNB = vinculoBNB;
                solicitacao.CodigoDoBem = request.CodigoDoBem;
                solicitacao.Numero = sequencial.Id;

                solicitacaoAppService.ValidarSolicitacao(solicitacao);
                solicitacaoAppService.Save(solicitacao, new List<AnexoDeSolicitacaoViewModel>());

                solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(x => x.Numero == solicitacao.Numero).Result;
                result.Payload = solicitacao.Numero;
            }
            else
                throw new ApplicationException("Usuário sem permissão de abertura de solicitação");

            return await Task.FromResult(result);
        }
    }
}
