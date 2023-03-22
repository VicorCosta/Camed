using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivo;
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

namespace Camed.SSC.Application.Requests.Solicitacao.Command.Salvar
{
    public class SalvarSolicitacaoCommandHandler : HandlerBase, IRequestHandler<SalvarSolicitacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identity;
        private readonly ISolicitacaoAppService solicitacaoAppService;
        private readonly ICaracter caracter;

        public SalvarSolicitacaoCommandHandler(IUnitOfWork uow, IIdentityAppService identity, ISolicitacaoAppService solicitacaoAppService, ICaracter caracter)
        {
            this.uow = uow;
            this.identity = identity;
            this.solicitacaoAppService = solicitacaoAppService;
            this.caracter = caracter;
        }

        public async Task<IResult> Handle(SalvarSolicitacaoCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var usuarioLogado = await uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == identity.Identity.Id);

                var entidade = new Domain.Entities.Solicitacao()
                {
                    Numero = request.Numero,
                    Atendente = uow.GetRepository<Usuario>().GetByIdAsync(request.Atendente_Id).Result,
                    Operador = uow.GetRepository<Usuario>().GetByIdAsync(request.Operador_Id).Result,
                    DataDeIngresso = request.DataDeIngresso,
                    Solicitante = uow.GetRepository<Domain.Entities.Solicitante>().QueryFirstOrDefaultAsync(w => w.Id == request.Solicitante_Id, includes: "Usuario").Result,
                    Agencia = uow.GetRepository<Agencia>().GetByIdAsync(request.Agencia_Id).Result,
                    Produtor = uow.GetRepository<Domain.Entities.Funcionario>().GetByIdAsync(request.Produtor_Id).Result,
                    TipoDeProduto = uow.GetRepository<TipoDeProduto>().GetByIdAsync(request.TipoDeProduto_Id).Result,
                    CotacaoSombrero = uow.GetRepository<CotacaoSombrero>().GetByIdAsync(request.CotacaoSombrero_Id).Result,
                    CanalDeDistribuicao = uow.GetRepository<CanalDeDistribuicao>().GetByIdAsync(request.CanalDeDistribuicao_Id).Result,
                    TipoDeSeguro = uow.GetRepository<TipoDeSeguro>().GetByIdAsync(request.TipoDeSeguro_Id).Result,
                    OperacaoDeFinanciamento = request.OperacaoDeFinanciamento,
                    DadosAdicionais = request.DadosAdicionais,
                    Segurado = uow.GetRepository<Segurado>().QueryFirstOrDefaultAsync(w => w.Id == request.Segurado_Id, includes: "VinculoBNB").Result,
                    Segmento = uow.GetRepository<Domain.Entities.Segmento>().GetByIdAsync(request.Segmento_Id).Result,
                    AreaDeNegocio = uow.GetRepository<AreaDeNegocio>().GetByIdAsync(request.AreaDeNegocio_Id).Result,
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
                };

                if (entidade.Segurado == null)
                {
                    throw new ApplicationException("É necessário informar o segurado");
                }
                if (entidade.Solicitante == null)
                {
                    throw new ApplicationException("É necessário informar o solicitante");
                }

                entidade.Solicitante = await AjustarSolicitante(entidade.Solicitante, request.Solicitante) ?
                    uow.GetRepository<Domain.Entities.Solicitante>().QueryFirstOrDefaultAsync(
                        w => w.Id == request.Solicitante_Id, includes: "Usuario").Result
                        : entidade.Solicitante;

                entidade.Segurado = await AjustarSegurado(entidade.Segurado, request.Segurado) ?
                    uow.GetRepository<Segurado>().QueryFirstOrDefaultAsync(
                        w => w.Id == request.Segurado_Id, includes: "VinculoBNB").Result
                        : entidade.Segurado;

                if (request.Id != 0)
                {
                    entidade.Id = request.Id;
                }

                //request.Id = 1;
                //usuarioLogado.PermitidoGerarCotacao ( ñ precisa de acordo com a documentação. )
                if (true || request.Id != 0)
                {
                    if (request.Id == 0)
                    {

                        var sequencial = new SequencialDeSolicitacao();
                        sequencial.Operador = usuarioLogado;
                        sequencial.Data = DateTime.Now;

                        await uow.GetRepository<SequencialDeSolicitacao>().AddAndSaveAsync(sequencial);
                        await uow.CommitAsync();

                        entidade.Numero = sequencial.Id;
                        entidade.Operador = usuarioLogado;
                        entidade.Solicitante.Usuario = usuarioLogado;

                        entidade.Aplicacao = "M";
                    }

                    solicitacaoAppService.Save(entidade, request.Anexos);

                    if(request.Numero == 0)
                    {
                        request.Numero = entidade.Numero;
                    }

                    entidade = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(x => x.Numero == request.Numero).Result;

                    result.Payload = entidade;
                }
                else
                    throw new ApplicationException("Usuário sem permissão de abertura de solicitação");

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> AjustarSolicitante(Domain.Entities.Solicitante solicitante_banco, Domain.Entities.Solicitante solicitante)
        {
            bool change = false;

            solicitante.TelefoneAdicional = solicitante.TelefoneAdicional != null ?
                caracter.RemoveSpecialCharacter2(solicitante.TelefoneAdicional) : solicitante.TelefoneAdicional;

            solicitante.TelefonePrincipal = solicitante.TelefonePrincipal != null ?
                caracter.RemoveSpecialCharacter2(solicitante.TelefonePrincipal) : solicitante.TelefonePrincipal;

            solicitante.TelefoneCelular = solicitante.TelefoneCelular != null ?
                caracter.RemoveSpecialCharacter2(solicitante.TelefoneCelular) : solicitante.TelefoneCelular;

            if (solicitante.Nome != solicitante_banco.Nome)
            {
                solicitante_banco.Nome = solicitante.Nome;
                change = true;
            }

            if (solicitante.TelefoneAdicional != solicitante_banco.TelefoneAdicional)
            {
                solicitante_banco.TelefoneAdicional = solicitante.TelefoneAdicional;
                change = true;
            }

            if (solicitante.TelefoneCelular != solicitante_banco.TelefoneCelular)
            {
                solicitante_banco.TelefoneCelular = solicitante.TelefoneCelular;
                change = true;
            }

            if (solicitante.TelefonePrincipal != solicitante_banco.TelefonePrincipal)
            {
                solicitante_banco.TelefonePrincipal = solicitante.TelefonePrincipal;
                change = true;
            }

            if (solicitante.Email != solicitante_banco.Email)
            {
                solicitante_banco.Email = solicitante.Email;
                change = true;
            }

            if (change)
            {
                await uow.GetRepository<Domain.Entities.Solicitante>().UpdateAndSaveAsync(solicitante_banco);
            }
            return change;
        }

        public async Task<bool> AjustarSegurado(Segurado segurado_banco, Segurado segurado)
        {
            bool change = false;

            segurado.TelefoneAdicional = segurado.TelefoneAdicional != null ?
                caracter.RemoveSpecialCharacter2(segurado.TelefoneAdicional) : segurado.TelefoneAdicional;

            segurado.TelefonePrincipal = segurado.TelefonePrincipal != null ?
                caracter.RemoveSpecialCharacter2(segurado.TelefonePrincipal) : segurado.TelefonePrincipal;

            segurado.TelefoneCelular = segurado.TelefoneCelular != null ?
                caracter.RemoveSpecialCharacter2(segurado.TelefoneCelular) : segurado.TelefoneCelular;

            if (segurado.Nome != segurado_banco.Nome)
            {
                segurado_banco.Nome = segurado.Nome;
                change = true;
            }

            if (segurado.TelefoneAdicional != segurado_banco.TelefoneAdicional)
            {
                segurado_banco.TelefoneAdicional = segurado.TelefoneAdicional;
                change = true;
            }

            if (segurado.TelefoneCelular != segurado_banco.TelefoneCelular)
            {
                segurado_banco.TelefoneCelular = segurado.TelefoneCelular;
                change = true;
            }

            if (segurado.TelefonePrincipal != segurado_banco.TelefonePrincipal)
            {
                segurado_banco.TelefonePrincipal = segurado.TelefonePrincipal;
                change = true;
            }

            if (segurado.Contato != segurado_banco.Contato)
            {
                segurado_banco.Contato = segurado.Contato;
                change = true;
            }

            if (segurado.CpfCnpj != segurado_banco.CpfCnpj)
            {
                segurado_banco.CpfCnpj = segurado.CpfCnpj;
                change = true;
            }

            if (segurado.Email != segurado_banco.Email)
            {
                segurado_banco.Email = segurado.Email;
                change = true;
            }

            if (segurado.EmailSecundario != segurado_banco.EmailSecundario)
            {
                segurado_banco.EmailSecundario = segurado.EmailSecundario;
                change = true;
            }

            if (segurado.VinculoBNB.Id != segurado_banco.VinculoBNB.Id)
            {
                segurado_banco.VinculoBNB = segurado.VinculoBNB;
                change = true;
            }

            if (change)
            {
                await uow.GetRepository<Segurado>().UpdateAndSaveAsync(segurado_banco);
            }
            return change;
        }
    }
}
