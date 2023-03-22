using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.CriarCopiaSolicitacao
{
    public class CriarCopiaSolicitacaoCommandHandler : HandlerBase, IRequestHandler<CriarCopiaSolicitacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly ISolicitacaoAppService solicitacaoAppService;
        private readonly IIdentityAppService identity;

        public CriarCopiaSolicitacaoCommandHandler(IUnitOfWork uow, ISolicitacaoAppService solicitacaoAppService, IIdentityAppService identity)
        {
            this.uow = uow;
            this.solicitacaoAppService = solicitacaoAppService;
            this.identity = identity;
        }

        public async Task<IResult> Handle(CriarCopiaSolicitacaoCommand request, CancellationToken cancellationToken)
        {
            var loggedUser = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == identity.Identity.Id).Result.Login;
            var entidade = new Domain.Entities.Solicitacao()
            {
                Numero = request.Numero,
                Atendente = uow.GetRepository<Usuario>().GetByIdAsync(request.Atendente_Id).Result,
                Operador = uow.GetRepository<Usuario>().GetByIdAsync(request.Operador_Id).Result,
                DataDeIngresso = request.DataDeIngresso,
                Solicitante = uow.GetRepository<Domain.Entities.Solicitante>().GetByIdAsync(request.Solicitante_Id).Result,
                Agencia = uow.GetRepository<Agencia>().GetByIdAsync(request.Agencia_Id).Result,
                Produtor = uow.GetRepository<Domain.Entities.Funcionario>().GetByIdAsync(request.Produtor_Id).Result,
                TipoDeProduto = uow.GetRepository<TipoDeProduto>().GetByIdAsync(request.TipoDeProduto_Id).Result,
                CanalDeDistribuicao = uow.GetRepository<CanalDeDistribuicao>().GetByIdAsync(request.CanalDeDistribuicao_Id).Result,
                TipoDeSeguro = uow.GetRepository<TipoDeSeguro>().GetByIdAsync(request.TipoDeSeguro_Id).Result,
                OperacaoDeFinanciamento = request.OperacaoDeFinanciamento,
                DadosAdicionais = request.DadosAdicionais,
                Segurado = uow.GetRepository<Segurado>().GetByIdAsync(request.Segurado_Id).Result,
                Segmento = uow.GetRepository<Domain.Entities.Segmento>().GetByIdAsync(request.Segmento_Id).Result,
                AreaDeNegocio = uow.GetRepository<AreaDeNegocio>().GetByIdAsync(request.AreaDeNegocio_Id).Result,
                Anexos = request.Anexos,
                Acompanhamentos = request.Acompanhamentos,
                AgendamentosDeLigacao = request.AgendamentosDeLigacao,
                SituacaoAtual = uow.GetRepository<Situacao>().GetByIdAsync(request.SituacaoAtual_Id).Result,
                DataHoraSituacaoAtual = request.DataHoraSituacaoAtual,
                Origem = request.Origem,
                CodigoDoBem = request.CodigoDoBem,
                NumeroFinanciamento = request.NumeroFinanciamento,
                Seguradora = uow.GetRepository<VW_SEGURADORA>().GetByIdAsync(request.Seguradora_Id).Result,
                Ramo = uow.GetRepository<Ramo>().GetByIdAsync(request.Ramo_Id).Result,
                Nu_Proposta_Seguradora = request.Nu_Proposta_Seguradora,
                TipoSeguroGS = uow.GetRepository<TipoDeSeguroGS>().GetByIdAsync(request.TipoSeguroGS_Id).Result,
                Nu_Apolice_Anterior = request.Nu_Apolice_Anterior,
                Pc_comissao = request.Pc_comissao,
                Co_Corretagem = request.Co_Corretagem,
                Pc_agenciamento = request.Pc_agenciamento,
                VL_IS = request.VL_IS,
                FL_Forma_Pagamento_1a = uow.GetRepository<FormaDePagamento>().GetByIdAsync(request.FL_Forma_Pagamento_1a_Id).Result,
                FL_Forma_Pagamento_Demais = uow.GetRepository<FormaDePagamento>().GetByIdAsync(request.FL_Forma_Pagamento_Demais_Id).Result,
                GrupoDeProducao = uow.GetRepository<GrupoDeProducao>().GetByIdAsync(request.GrupoDeProducao_Id).Result,
                TipoDeCategoria = uow.GetRepository<TipoDeCategoria>().GetByIdAsync(request.TipoDeCategoria_Id).Result,
                Sede_Envia_Doc_Fisico = request.Sede_Envia_Doc_Fisico,
                Nu_Sol_Vistoria = request.Nu_Sol_Vistoria,
                Cadastrado_GS = request.Cadastrado_GS,
                Cd_estudo = request.Cd_estudo,
                estudo_origem = request.estudo_origem,
                CheckList = request.CheckList,
                Indicacoes = request.Indicacoes,
                TipoDeCancelamento = uow.GetRepository<TipoDeCancelamento>().GetByIdAsync(request.TipoDeCancelamento_Id).Result,
                DataFimVigencia = request.DataFimVigencia,
                QtdDiasSLARenovacao = request.QtdDiasSLARenovacao,
                TipoEndosso = request.TipoEndosso,
                MotivoEndossoCancelamento = uow.GetRepository<MotivoEndossoCancelamento>().GetByIdAsync(request.MotivoEndossoCancelamento_Id).Result,
                MotivoRecusa = uow.GetRepository<MotivoRecusa>().GetByIdAsync(request.MotivoRecusa_Id).Result,
                VIP = request.VIP,
                OrcamentoPrevio = request.OrcamentoPrevio,
                CROSSUP = request.CROSSUP,
                Mercado = request.Mercado,
                Rechaco = request.Rechaco,
                vlr_premiotot_anterior = request.vlr_premiotot_anterior,
                perc_comissao_anterior = request.perc_comissao_anterior,
                vlr_premiotot_atual = request.vlr_premiotot_atual,
                perc_comissao_atual = request.perc_comissao_atual,
                VistoriaNec = request.VistoriaNec,
                ObsVistoria = request.ObsVistoria,
                AgenciaConta = uow.GetRepository<Agencia>().GetByIdAsync(request.AgenciaConta_Id).Result,
                TipoComissaoRV = request.TipoComissaoRV,
                Checkins = request.Checkins,
                Aplicacao = request.Aplicacao,
                SeguradoraCotacao = uow.GetRepository<VW_SEGURADORA>().GetByIdAsync(request.SeguradoraCotacao_Id).Result,
                Rastreador = request.Rastreador,
                DataVencimento1aParc = request.DataVencimento1aParc,
                vlr_premiotot_prop = request.vlr_premiotot_prop,
                Id = request.Id
            };

            solicitacaoAppService.SaveCopy(entidade, loggedUser);

            result.Payload = request.Numero;

            return await Task.FromResult(result);
        }
    }
}
