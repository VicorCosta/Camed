using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.Util;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Camed.SSC.Application.Services
{
    public class AcompanhamentoAppService : IAcompanhamentoAppService
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identity;
        private readonly ISolicitacaoAppService solicitacaoAppService;
        private readonly IUsuarioAppService usuarioAppService;
        private readonly IFrameAppService frameAppService;
        private readonly IFuncionarioAppService funcionarioAppService;
        private readonly SSCContext contextoDeDados;
        private readonly ICaracter caracter;

        public AcompanhamentoAppService(IUnitOfWork uow, IIdentityAppService identity, 
            ISolicitacaoAppService solicitacaoAppService, IUsuarioAppService usuarioAppService, 
            IFrameAppService frameAppService, IFuncionarioAppService funcionarioAppService, 
            SSCContext contextoDeDados, ICaracter caracter)
        {
            this.uow = uow;
            this.identity = identity;
            this.solicitacaoAppService = solicitacaoAppService;
            this.usuarioAppService = usuarioAppService;
            this.frameAppService = frameAppService;
            this.funcionarioAppService = funcionarioAppService;
            this.contextoDeDados = contextoDeDados;
            this.caracter = caracter;
        }
        public Acompanhamento ObterUltimoAcompanhamento(Solicitacao solicitacao)
        {
            //var acompanhamento = uow.GetRepository<Acompanhamento>().QueryAsync(x => x.Solicitacao.Id == solicitacao.Id,
            //    includes: new[] { "Anexos", "Atendente", "Grupo", "Solicitacao", "Situacao", "VendasCompartilhadas" })
            //    .Result.OrderByDescending(w => w.DataEHora).FirstOrDefault();

            var acompanhamento = uow.GetRepository<Acompanhamento>().QueryAsync(x => x.Solicitacao.Id == solicitacao.Id,
                includes: new[] { "Solicitacao", "Situacao", "Atendente", "Grupo", })
                .Result.OrderByDescending(w => w.DataEHora).FirstOrDefault();

            return acompanhamento;
        }

        public void AdicionarAcompanhamento(AcompanhamentoViewModel model, string login = null)
        {
            int idDaSolicitacao, idDaAcao;
            bool cadastrado_gs, permiteVisualizarObservacao, permiteVisualizarAnexo, enviaEmailAoSegurado;
            bool? sede_envia_doc_fisico, vip, crossup, vistorianec, rastreador;
            string observacao, codigoDoBem, numeroFinanciamento;
            DateTime? datavencimento1aparc;
            string nu_proposta_seguradora, TipoComissaoRV;
            int? seguradora, ramo, segmento, produtor;
            int? fl_forma_pagamento_1a, fl_forma_pagamento_demais, grupodeproducao, tipoDeCategoriaId, tipoDeProdutoId;
            int? nu_sol_vistoria, tipoSeguroGS, motivoRecusa_Id, idMotivoEndossoCancelamento;
            string nu_apolice_anterior;
            decimal? pc_comissao, co_corretagem, pc_agenciamento, vl_is, cd_estudo, perc_comissao_atual, vlr_premiotot_atual, vlr_premiotot_prop;
            string tipoEndosso;
            string loginUsuarioLogado;
            int? produtorId, tipoDeProduto_Id;
            int? seguradoracotacao;
            var anexos = model.Anexos;
            
            loginUsuarioLogado = login ?? uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(
                w => w.Id == identity.Identity.Id).Result.Login;

            if (model.Acao == null)
            {
                throw new Exception("Favor selecionar a ação!");
            }
            else
            {
                idDaAcao = model.Acao.Id;
            }

            Frame frameCapaProposta = frameAppService.ListaPorNome("Capa da Proposta");

            if (frameCapaProposta.AcoesAcompanhamento.Any(x => x.Frame.Id == idDaAcao) && model.DataVencimento1aParc == null)
            {
                if (model.FL_Forma_Pagamento_1a != 6 && model.FL_Forma_Pagamento_1a != 7)
                    throw new Exception("Favor preencher o campo 'Data Vencimento 1a Parc'.");
            }


            if (frameCapaProposta.AcoesAcompanhamento.Any(x => x.Frame.Id == idDaAcao))
            {
                if ((model.GrupoDeProducao == null) || (model.GrupoDeProducao == 0))
                    throw new Exception("Favor preencher o campo 'Grupo De Producao'.");
            }

            if (frameCapaProposta.AcoesAcompanhamento.Any(x => x.Frame.Id == idDaAcao) && model.Co_Corretagem == null && model.CanalDeDistribuicao_Id == 3)
            {
                throw new Exception("Favor preencher o campo 'Co_Corretagem'.");
            }

            if (idDaAcao == 11 || idDaAcao == 48)
            {
                var emailPrincipalSegurado = model.Email;
                var emailSecundarioSegurado = model.EmailSecundario;
                var celularPrincipal = model.TelefoneCelular;
                var telefonePrincipal = model.TelefonePrincipal;
                var telefoneSecundario = model.TelefoneAdicional;

                if (!string.IsNullOrEmpty(celularPrincipal))
                {
                    celularPrincipal = celularPrincipal.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                }

                if (!string.IsNullOrEmpty(telefonePrincipal))
                {
                    telefonePrincipal = telefonePrincipal.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                }

                if (!string.IsNullOrEmpty(telefoneSecundario))
                {
                    telefoneSecundario = telefoneSecundario.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                }

                var validacaoEmail = false;
                var validacaoCelular = false;

                var regexCelular = new Regex(@"^([1-9]{2})([9]{1})([7-9]{1})([0-9]{7})$");
                var regexEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

                celularPrincipal = celularPrincipal == null ? "" : celularPrincipal;
                telefonePrincipal = telefonePrincipal == null ? "" : telefonePrincipal;
                telefoneSecundario = telefoneSecundario == null ? "" : telefoneSecundario;
                emailPrincipalSegurado = emailPrincipalSegurado == null ? "" : emailPrincipalSegurado;
                emailSecundarioSegurado = emailSecundarioSegurado == null ? "" : emailSecundarioSegurado;

                try
                {
                    if (regexCelular.IsMatch(celularPrincipal) || regexCelular.IsMatch(telefonePrincipal) || regexCelular.IsMatch(telefoneSecundario))
                    {
                        validacaoCelular = true;
                    }
                    if (regexEmail.IsMatch(emailPrincipalSegurado) || regexEmail.IsMatch(emailSecundarioSegurado))
                    {
                        validacaoEmail = true;
                    }

                    if (!validacaoCelular && !validacaoEmail)
                    {
                        throw new ApplicationException("O segurado não possui um e-mail ou um celular válido cadastrado. Favor corrigir o cadastro antes do aceite da cotação.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

            idDaSolicitacao = model.Solicitacao_Id;

            cadastrado_gs = model.Cadastrado_GS;
            cd_estudo = model.Cd_estudo;
            codigoDoBem = model.CodigoDoBem == null ? null : model.CodigoDoBem.Replace(",", "/");
            fl_forma_pagamento_1a = model.FL_Forma_Pagamento_1a;
            fl_forma_pagamento_demais = model.FL_Forma_Pagamento_Demais;
            grupodeproducao = model.GrupoDeProducao;

            tipoDeCategoriaId = model.TipoDeCategoria;

            tipoDeProdutoId = model.TipoDeProduto_Id;

            nu_apolice_anterior = model.Nu_Apolice_Anterior;
            nu_proposta_seguradora = model.Nu_Proposta_Seguradora;
            nu_sol_vistoria = model.Nu_Sol_Vistoria;
            numeroFinanciamento = model.NumeroFinanciamento;
            observacao = model.Observacao;
            pc_agenciamento = model.Pc_agenciamento;
            pc_comissao = model.Pc_comissao;

            co_corretagem = model.Co_Corretagem;
            permiteVisualizarObservacao = model.PermiteVisualizarObservacao;
            permiteVisualizarAnexo = model.PermiteVisualizarAnexo;

            perc_comissao_atual = model.perc_comissao_atual;
            vlr_premiotot_atual = model.vlr_premiotot_atual;
            vlr_premiotot_prop = model.vlr_premiotot_prop;

            if (string.IsNullOrEmpty(model.Sede_Envia_Doc_Fisico))
            {
                sede_envia_doc_fisico = null;
            }
            else
            {
                sede_envia_doc_fisico = model.Sede_Envia_Doc_Fisico == "1" ? true : false;
            }

            tipoSeguroGS = model.TipoSeguroGS;
            vl_is = model.VL_IS;

            if (string.IsNullOrEmpty(model.VIP))
            {
                vip = null;
            }
            else
            {
                vip = model.VIP == "1" ? true : false;
            }

            if (string.IsNullOrEmpty(model.CROSSUP))
            {
                crossup = null;
            }
            else
            {
                crossup = model.CROSSUP == "1" ? true : false;
            }

            tipoEndosso = model.TipoEndosso;

            if (model.MotivoEndossoCancelamento == null)
                idMotivoEndossoCancelamento = null;
            else
                idMotivoEndossoCancelamento = model.MotivoEndossoCancelamento;

            if (model.MotivoRecusa == null)
                motivoRecusa_Id = null;
            else
                motivoRecusa_Id = model.MotivoRecusa;

            if (model.Seguradora == null)
                seguradora = null;
            else
                seguradora = model.Seguradora.Id == 0 ? null : (int?)model.Seguradora.Id;

            if (model.SeguradoraCotacao == null)
                seguradoracotacao = null;
            else
                seguradoracotacao = model.SeguradoraCotacao.Id == 0 ? null : (int?)model.SeguradoraCotacao.Id;

            if (model.Ramo == null)
                ramo = null;
            else
                ramo = model.Ramo.Id == 0 ? null : (int?)model.Ramo.Id;

            if (model.Produtor == null)
                produtor = null;
            else
                produtor = model.Produtor.Id == 0 ? null : (int?)model.Produtor.Id;

            if (model.Segmento == null)
                segmento = null;
            else
                segmento = model.Segmento.Id == 0 ? null : (int?)model.Segmento.Id;

            var numeroDaSolicitacao = solicitacaoAppService.ObterNumero(idDaSolicitacao);

            var gruposDoUsuarioLogado = usuarioAppService.ObterGrupos(loginUsuarioLogado);
            var vendaCompartilhada = new List<VendaCompartilhada>();

            if (model.eVendaComp)
            {
                foreach (var v in model.dadosVendaComp)
                {
                    vendaCompartilhada.Add(new VendaCompartilhada()
                    {
                        Id = 0,
                        Produtor = new Usuario { Id = v.Produtor.Id },
                        Percentual = v.Percentual
                    });
                }
            }

            if (string.IsNullOrEmpty(model.VistoriaNec))
            {
                vistorianec = null;
            }
            else
            {
                vistorianec = model.VistoriaNec == "1" ? true : false;
            }

            if (string.IsNullOrEmpty(model.Rastreador))
            {
                rastreador = null;
            }
            else
            {
                rastreador = model.Rastreador == "1" ? true : false;
            }

            if (model.DataVencimento1aParc == null)
            {
                datavencimento1aparc = null;
            }
            else
            {
                datavencimento1aparc = model.DataVencimento1aParc;
            }


            if (string.IsNullOrEmpty(model.TipoComissaoRV))
                TipoComissaoRV = null;
            else
                TipoComissaoRV = model.TipoComissaoRV;

            produtorId = model.Produtor == null ? null : model.Produtor.Id == 0 ? null : (int?)model.Produtor.Id;

            var usuarioAtual = uow.GetRepository<Usuario>()
                .QueryFirstOrDefaultAsync(w => w.Login == loginUsuarioLogado).Result;

            Frame frameProdutorBnb = frameAppService.ListaPorNome("Produtor BNB");
            if (produtor != null && frameProdutorBnb.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
            {
                var produtorUsuarioLogado = funcionarioAppService.GetFuncionarioPorMatricula(usuarioAtual.Matricula, true);
                if (produtorUsuarioLogado != null && produtor.Value != produtorUsuarioLogado.Id)
                {
                    produtor = produtorUsuarioLogado.Id;
                    produtorId = produtorUsuarioLogado.Id;
                }
            }

            DateTime agora = DateTime.Now;
            bool enviaSMSSegurado = false;

            enviaEmailAoSegurado = model.PermiteEmailAoSegurado == "1" ? true : false;
            Cadastrar(idDaSolicitacao, idDaAcao, gruposDoUsuarioLogado, observacao,
                permiteVisualizarObservacao, anexos, usuarioAtual, codigoDoBem, numeroFinanciamento,
                seguradora, ramo, nu_proposta_seguradora, tipoSeguroGS,
                nu_apolice_anterior, pc_comissao, co_corretagem, pc_agenciamento, vl_is,
                fl_forma_pagamento_1a, fl_forma_pagamento_demais, grupodeproducao, tipoDeCategoriaId, tipoDeProdutoId,
                sede_envia_doc_fisico, nu_sol_vistoria, cadastrado_gs, cd_estudo,
                segmento, produtor, vendaCompartilhada, tipoEndosso, idMotivoEndossoCancelamento, motivoRecusa_Id, vip, crossup, agora,
                ref enviaSMSSegurado, model.Rechaco, vlr_premiotot_atual, perc_comissao_atual, vistorianec, 
                model.ObsVistoria, TipoComissaoRV, produtorId, seguradoracotacao, vlr_premiotot_prop, 
                permiteVisualizarAnexo, rastreador, datavencimento1aparc, enviaEmailAoSegurado);

            Segurado referenciaSMSSegurado = uow.GetRepository<Solicitacao>()
                .QueryFirstOrDefaultAsync(x => x.Id == idDaSolicitacao, includes: new[] { "Segurado" })
                .Result.Segurado;

            if (referenciaSMSSegurado != null && enviaSMSSegurado)
            {
                Regex rgx = new Regex(@"^([1-9]{2})([9]{1})([7-9]{2})([0-9]{6})$");

                dynamic TelefonePrincipal = referenciaSMSSegurado.TelefonePrincipal == null ?
                    string.Empty :
                    referenciaSMSSegurado.TelefonePrincipal;

                TelefonePrincipal = TelefonePrincipal.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                dynamic TelefoneCelular = referenciaSMSSegurado.TelefoneCelular == null ?
                    string.Empty :
                    referenciaSMSSegurado.TelefoneCelular;

                TelefoneCelular = TelefoneCelular.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                dynamic telefoneDestino = rgx.IsMatch(TelefonePrincipal) ?
                                                referenciaSMSSegurado.TelefonePrincipal :
                                         rgx.IsMatch(TelefoneCelular) ?
                                                referenciaSMSSegurado.TelefoneCelular :
                                                string.Empty;

                int ultimaOrdemAcomp = uow.GetRepository<Acompanhamento>().QueryAsync(x => x.Solicitacao.Id == idDaSolicitacao)
                    .Result.Max(x => x.Ordem);

                ultimaOrdemAcomp = ultimaOrdemAcomp - 1;

                int situacaoAnterior = uow.GetRepository<Acompanhamento>().QueryFirstOrDefaultAsync(x => 
                x.Solicitacao.Id == idDaSolicitacao && x.Ordem == ultimaOrdemAcomp, 
                includes: new[] { "Situacao" }).Result.Situacao.Id;

                int? parametroSMS_Id = uow.GetRepository<MapeamentoAcaoSituacao>().QueryFirstOrDefaultAsync(x => 
                x.Acao.Id == idDaAcao && x.SituacaoAtual.Id == situacaoAnterior).Result.ParametroSistemaSMS.Id;

                try
                {
                    if (telefoneDestino != string.Empty && parametroSMS_Id != null)
                    {
                        dynamic textoSMSParametroSist = uow.GetRepository<ParametrosSistema>()
                                                        .QueryFirstOrDefaultAsync(x => x.Id == parametroSMS_Id).Result
                                                        .Valor.Replace("[NomeSegurado]", referenciaSMSSegurado.Nome)
                                                        .Replace("[NumeroSolicitacao]", numeroDaSolicitacao.ToString())
                                                        .Replace("[getdate]", agora.ToString("dd/MM/yyyy HH:mm"));

                        dynamic returnedString = "";
                        dynamic user = "syscamed.corretora";
                        dynamic pass = "C@m3d.2016.975";
                        dynamic URL = "http://api.infobip.com/sms/1/text/single";

                        telefoneDestino = "55" + telefoneDestino;

                        dynamic data = JsonConvert.SerializeObject(new MessageSending("Sistema SSC", telefoneDestino, textoSMSParametroSist));

                        HttpClientHandler handler = new HttpClientHandler();
                        handler.UseDefaultCredentials = true;
                        HttpClient client = new HttpClient(handler);
                        client.BaseAddress = new Uri(URL);
                        byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + pass);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpContent content = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                        HttpResponseMessage message = client.PostAsync(URL, content).Result;

                        if (!message.IsSuccessStatusCode)
                        {
                            List<AnexosDeInbox> lst = new List<AnexosDeInbox>();
                            Inbox logigor = new Inbox()
                            {
                                Id = 0,
                                Assunto = "log inbox administração",
                                Texto = message.ToString(),
                                UsuarioRemetente = null,
                                UsuarioDestinatario = uow.GetRepository<Usuario>()
                                .QueryFirstOrDefaultAsync(w => w.Id == 9669).Result,
                                DataCriacao = DateTime.Now,
                                VisivelEntrada = true,
                                VisivelSaida = true,
                                Lida = false,
                                Anexos = lst
                            };

                            Inbox logrommel = new Inbox()
                            {
                                Id = 0,
                                Assunto = "log inbox administração",
                                Texto = message.ToString(),
                                UsuarioRemetente = null,
                                UsuarioDestinatario = uow.GetRepository<Usuario>()
                                .QueryFirstOrDefaultAsync(w => w.Id == 2238).Result,
                                DataCriacao = DateTime.Now,
                                VisivelEntrada = true,
                                VisivelSaida = true,
                                Lida = false,
                                Anexos = lst
                            };

                            uow.GetRepository<Inbox>().AddAndSaveAsync(logigor);
                            uow.GetRepository<Inbox>().AddAndSaveAsync(logrommel);
                            uow.CommitAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw ex;
                }
            }
        }

        public void Cadastrar(int idDaSolicitacao, int idDaAcao, List<Grupo> gruposDoUsuario, string observacao, bool permiteVisualizarObservacao, List<AnexoDeAcompanhamento> anexos,
                      dynamic usuarioAtual, string codigoDoBem, string numeroFinanciamento, int? seguradora, int? ramo, string nu_proposta_seguradora, int? tipoSeguroGS,
                      string nu_apolice_anterior, decimal? pc_comissao, decimal? co_corretagem, decimal? pc_agenciamento, decimal? vl_is, int? fl_forma_pagamento_1a, int? fl_forma_pagamento_demais,
                      int? grupodeproducao, int? tipoDeCategoriaId, int? tipoDeProduto_Id, bool? sede_envia_doc_fisico, int? nu_sol_vistoria, bool cadastrado_gs, decimal? cd_estudo, int? segmento,
                      int? produtor, List<VendaCompartilhada> vendaCompartilhada, string tipoEndosso, int? motivoEndossoCancelamento, int? motivoRecusa, bool? vip, bool? crossup, DateTime agora,
                      ref bool enviaSMSsegurado, bool rechaco, decimal? vlr_premiotot_atual, decimal? perc_comissao_atual, bool? VistoriaNec, string ObsVistoria, string TipoComissaoRV, int? produtorId,
                      int? seguradoracotacao, decimal? vlr_premiotot_prop, bool permiteVisualizarAnexo, bool? Rastreador, DateTime? DataVencimento1aParc, bool? permiteEmailAoSegurado)
        {
            var solicitacao = uow.GetRepository<Solicitacao>().GetByIdAsync(idDaSolicitacao).Result;

            if (gruposDoUsuario == null || gruposDoUsuario.Count() == 0)
                throw new ArgumentException("O usuário cadastrando o acompanhamento deve estar associado a um grupo",
                    "gruposDoUsuario");

            MapeamentoAcaoSituacao mapeamento = ObterMapeamentoAcaoSituacao(idDaSolicitacao, idDaAcao, gruposDoUsuario);

            enviaSMSsegurado = mapeamento.EnviaSMSAoSegurado;

            if (mapeamento.ExigeEnvioDeArquivo && anexos.Count.Equals(0))
                throw new ArgumentException("Para salvar o acompanhamento é necessário informar, pelo menos, um arquivo.");

            if (mapeamento.ExigeObservacao && string.IsNullOrEmpty(observacao))
                throw new ArgumentException("É necessário informar uma observação para cadastrar o acompanhamento.");

            var frameCapaDaProposta = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Id == 2, includes: new[] { "AcoesAcompanhamento" }).Result;

            #region Validação Capa da Proposta

            if (frameCapaDaProposta.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
            {
                if (anexos.Where(x => x.Nome.ToLower().StartsWith("proposta") &&
                                      x.Nome.ToLower().EndsWith(".pdf")).Count() == 0)
                    throw new ApplicationException(
                        "É preciso informar um arquivo anexo (.pdf) cujo nome comece com \"proposta\"");
                else if (anexos.Where(x => x.Nome.ToLower().StartsWith("proposta") &&
                                           x.Nome.ToLower().EndsWith(".pdf")).Count() > 1)
                    throw new ApplicationException(
                        "Deve ser enviado apenas um arquivo anexo (.pdf) cujo nome comece com \"proposta\"");

                if ((segmento == null || segmento <= 0) && solicitacao.Segmento == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Segmento");

                if (seguradora == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Seguradora");

                if (ramo == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Ramo");

                if (string.IsNullOrEmpty(nu_proposta_seguradora))
                    throw new ApplicationException("Informar dados da capa da proposta: Nº da Proposta");

                if (tipoSeguroGS == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Tipo de Seguro");

                if (string.IsNullOrEmpty(nu_apolice_anterior))
                    if (tipoSeguroGS == 1 /* Endosso */|| tipoSeguroGS == 5 /* Renovação */)
                        throw new ApplicationException("Para seguros do tipo Endosso ou Renovação, " +
                            "é obrigatório informar o número da apólice anterior");

                if (pc_comissao == null || pc_comissao < 0)
                    throw new ApplicationException("Informar dados da capa da proposta: Comissão(%)");

                if (vl_is == null || vl_is <= 0)
                    throw new ApplicationException("Informar dados da capa da proposta: Valor da IS");

                if (fl_forma_pagamento_1a == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Forma Pagto 1ª");

                if (grupodeproducao == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Grupo De Produção");

                if (ExisteTipoDeCategoriaPorTipoDeProduto(tipoDeProduto_Id))
                {
                    if (tipoDeCategoriaId == null)
                        throw new ApplicationException("Informar o tipo de Categoria");
                }

                if (vlr_premiotot_prop == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Valor Premio Líquido");

                if (sede_envia_doc_fisico == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Sede Envia Doc Físico.");

                if (vip == null)
                    throw new ApplicationException("Informar dados da capa da proposta: Vip.");

                if (vendaCompartilhada.Count == 1)
                    throw new ApplicationException("Venda compartilhada deve conter pelo menos dois produtores.");

                if (vendaCompartilhada != null && vendaCompartilhada.Count > 0)
                {
                    decimal somaPercentual = 0;
                    foreach (var venda in vendaCompartilhada)
                    {
                        if (venda.Percentual <= 0)
                        {
                            throw new ApplicationException("O percentual da venda compartilhada deve ser maior que zero.");
                        }
                        somaPercentual += venda.Percentual;
                    }

                    if (somaPercentual != 100)
                        throw new ApplicationException("O total do percentual da venda compartilhada deve ser 100%");
                }
                if (VistoriaNec == null)
                {
                    throw new ApplicationException("Favor informar se a vistoria é necessária.");
                }
                else
                {
                    solicitacao.VistoriaNec = VistoriaNec;

                    if (VistoriaNec.Value == true)
                    {
                        if (string.IsNullOrEmpty(ObsVistoria))
                            throw new ApplicationException("Favor informar a observação da vistoria.");

                        solicitacao.ObsVistoria = ObsVistoria;
                    }
                }

                if (Rastreador == null)
                {
                    throw new ApplicationException("Favor informar se o rastreador é necessário.");
                }
                else
                {
                    solicitacao.Rastreador = Rastreador;
                }

                solicitacao.DataVencimento1aParc = DataVencimento1aParc;

                if (string.IsNullOrEmpty(TipoComissaoRV))
                    throw new ApplicationException("Informar o Tipo de Comissão da RV.");

                if (solicitacao == null)
                    throw new ApplicationException("Não foi possível cadastrar o acompanhamento, " +
                        "pois a solicitação passada não foi encontrada na base de dados.");

                Frame frameCapa = frameAppService.ListaPorNome("Capa da Proposta");
                if (vendaCompartilhada.Count <= 0 && frameCapa.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
                {
                    if (solicitacao.TipoDeProduto.Nome.Equals("SEGUROS RURAIS"))
                    {
                        var tipodeAgencias = uow.GetRepository<TipoDeAgencia>().GetByIdAsync(solicitacao.Agencia.Id).Result;
                        if (tipodeAgencias.Nome.Equals("BNB AGENTES") && vendaCompartilhada.Count <= 0)
                        {
                            throw new ApplicationException("Venda compartilhada obrigatória");
                        }
                    }
                }

                int ret = 0;
                string msg = "", sql_proc;
                SqlParameter[] parametros;

                if (nu_apolice_anterior != null)
                {
                    sql_proc = "dbo.ValidaNuApoliceAnterior " +
                        " @CD_SEGURADORA, @NU_CPFCGC_SEGURADO, @NU_APOLICE_ANTERIOR, @MSG OUTPUT";

                    parametros = new SqlParameter[] {
                            new SqlParameter("@CD_SEGURADORA", SqlDbType.Decimal){Value = seguradora},
                            new SqlParameter("@NU_CPFCGC_SEGURADO", SqlDbType.Decimal){Value = solicitacao.Segurado.CpfCnpj},
                            new SqlParameter("@NU_APOLICE_ANTERIOR", SqlDbType.VarChar, 20){Value = nu_apolice_anterior},
                            new SqlParameter("@MSG", SqlDbType.VarChar, 255){Direction = ParameterDirection.Output},
                            new SqlParameter("@RET", SqlDbType.Int){Direction = ParameterDirection.ReturnValue}
                        };

                    contextoDeDados.Database.ExecuteSqlCommand(sql_proc, parametros);

                    ret = (int)parametros[4].Value;
                    msg = parametros[3].Value.ToString();

                    if (ret == 0 && !string.IsNullOrEmpty(msg))
                    {
                        throw new ApplicationException(msg);
                    }
                }

                sql_proc = "dbo.ValidaNuProposta @CD_SEGURADORA, @NU_PROPOSTA_SEGURADORA, @MSG OUTPUT";

                parametros = new SqlParameter[] {
                        new SqlParameter("@CD_SEGURADORA", SqlDbType.Decimal){Value = seguradora},
                        new SqlParameter("@NU_PROPOSTA_SEGURADORA", SqlDbType.VarChar, 20){Value = nu_proposta_seguradora},
                        new SqlParameter("@MSG", SqlDbType.VarChar, 255){Direction = ParameterDirection.Output},
                        new SqlParameter("@RET", SqlDbType.Int){Direction = ParameterDirection.ReturnValue}
                    };

                contextoDeDados.Database.ExecuteSqlCommand(sql_proc, parametros);

                ret = (int)parametros[3].Value;
                msg = parametros[2].Value.ToString();

                if (ret == 0 && !string.IsNullOrEmpty(msg))
                {
                    throw new ApplicationException(msg);
                }

                sql_proc = "dbo.ValidaPCAgenciamento @TIPODEPRODUTO_ID, @PC_AGENCIAMENTO, @MSG OUTPUT";
                object val_pc_agenciamento = (pc_agenciamento == null ? System.DBNull.Value : (object)pc_agenciamento);

                parametros = new SqlParameter[] {
                        new SqlParameter("@TIPODEPRODUTO_ID", SqlDbType.Int){Value = solicitacao.TipoDeProduto.Id},
                        new SqlParameter("@PC_AGENCIAMENTO", SqlDbType.Decimal){Value = val_pc_agenciamento},
                        new SqlParameter("@MSG", SqlDbType.VarChar, 255){Direction = ParameterDirection.Output},
                        new SqlParameter("@RET", SqlDbType.Int){Direction = ParameterDirection.ReturnValue}
                    };

                contextoDeDados.Database.ExecuteSqlCommand(sql_proc, parametros);

                ret = (int)parametros[3].Value;
                msg = parametros[2].Value.ToString();

                if (ret == 0 && !string.IsNullOrEmpty(msg))
                {
                    throw new ApplicationException(msg);
                }

                sql_proc = "dbo.ValidaPCComisao @TIPODEPRODUTO_ID, @PC_COMISSAO, @MSG OUTPUT";

                parametros = new SqlParameter[] {
                        new SqlParameter("@TIPODEPRODUTO_ID", SqlDbType.Int){Value = solicitacao.TipoDeProduto.Id},
                        new SqlParameter("@PC_COMISSAO", SqlDbType.Decimal){Value = pc_comissao},
                        new SqlParameter("@MSG", SqlDbType.VarChar, 255){Direction = ParameterDirection.Output},
                        new SqlParameter("@RET", SqlDbType.Int){Direction = ParameterDirection.ReturnValue}
                    };

                contextoDeDados.Database.ExecuteSqlCommand(sql_proc, parametros);

                ret = (int)parametros[3].Value;
                msg = parametros[2].Value.ToString();

                if (ret == 0 && !string.IsNullOrEmpty(msg))
                {
                    throw new ApplicationException(msg);
                }

                sql_proc = "dbo.ValidaValorIS @TIPODEPRODUTO_ID, @VL_IS, @MSG OUTPUT";

                parametros = new SqlParameter[] {
                        new SqlParameter("@TIPODEPRODUTO_ID", SqlDbType.Int){Value = solicitacao.TipoDeProduto.Id},
                        new SqlParameter("@VL_IS", SqlDbType.Decimal){Value = vl_is},
                        new SqlParameter("@MSG", SqlDbType.VarChar, 255){Direction = ParameterDirection.Output},
                        new SqlParameter("@RET", SqlDbType.Int){Direction = ParameterDirection.ReturnValue}
                    };

                contextoDeDados.Database.ExecuteSqlCommand(sql_proc, parametros);

                ret = (int)parametros[3].Value;
                msg = parametros[2].Value.ToString();

                if (ret == 0 && !string.IsNullOrEmpty(msg))
                {
                    throw new ApplicationException(msg);
                }

                solicitacao.Rechaco = rechaco;
            }

            #endregion Validação Capa da Proposta

            try
            {
                if (solicitacao == null)
                    throw new ApplicationException("Não foi possível cadastrar o acompanhamento, " +
                        "pois a solicitação passada não foi encontrada na base de dados.");

                if (produtorId != null && produtorId.Value != 0)
                    solicitacao.Produtor.Id = produtorId.Value;

                //AtualizaSLAAcompanhamentoAnterior(solicitacao, agora);
                Acompanhamento ultimoAcompanhamento = AtualizaSLAAcompanhamentoAnterior(solicitacao, agora);

                //int ordem = solicitacao.Acompanhamentos.Count() + 1;
                int ordem = ultimoAcompanhamento.Ordem + 1;

                List<Grupo> grupos = uow.GetRepository<MapeamentoAcaoSituacao>().QueryAsync(x =>
                    x.SituacaoAtual.Id == mapeamento.ProximaSituacao.Id, includes: new[] { "Grupo" })
                    .Result.Select(x => x.Grupo).Distinct().ToList();

                bool EFimFluxo = uow.GetRepository<Situacao>()
                    .QueryFirstOrDefaultAsync(x => x.Id == mapeamento.ProximaSituacao.Id).Result.EFimFluxo;

                Grupo grupo = null;
                string tempoSLAEfet;

                switch (EFimFluxo)
                {
                    case true:
                        tempoSLAEfet = "0h00m";
                        if (grupos.Count == 1)
                        {
                            grupo = grupos.FirstOrDefault();
                        }
                        break;

                    case false:
                        tempoSLAEfet = null;
                        grupo = grupos.FirstOrDefault();
                        break;

                    default:
                        throw new ApplicationException(
                            "Foram encontrados dois ou mais grupos responsáveis pelo próximo acompanhamento.");
                }

                var novoAcompanhamento = new Acompanhamento()
                {
                    Id = 0,
                    Ordem = ordem,
                    DataEHora = agora,
                    Observacao = observacao,
                    PermiteVisualizarObservacao = permiteVisualizarObservacao,
                    PermiteVisualizarAnexo = permiteVisualizarAnexo,
                    Situacao = mapeamento.ProximaSituacao,
                    Atendente = usuarioAtual,
                    Solicitacao = solicitacao,
                    Grupo = grupo,
                    TempoSLADef = mapeamento.ProximaSituacao.TempoSLA,
                    TempoSLAEfet = tempoSLAEfet,
                    EVendaCompartilhada = (vendaCompartilhada.Count > 0),
                    PermiteEmailAoSegurado = permiteEmailAoSegurado
                };

                if (solicitacao.Atendente != null && solicitacao.Atendente_Id != 0 && solicitacao.Id != 0)
                {
                    var existeRegistro = solicitacaoAppService.ExisteRegistroSolicitacaoAtendente(
                        solicitacao.Id, solicitacao.Atendente_Id);

                    if (!existeRegistro)
                    {
                        var solicitacaoMovimentacao = new SolicitacaoMovimentacaoRamo();
                        solicitacaoMovimentacao.Solicitacao = solicitacao;
                        solicitacaoMovimentacao.Atendente = solicitacao.Atendente;
                        uow.GetRepository<SolicitacaoMovimentacaoRamo>().AddAndSaveAsync(solicitacaoMovimentacao);
                    }
                }

               uow.GetRepository<Acompanhamento>().AddAndSaveAsync(novoAcompanhamento);
               System.Threading.Thread.Sleep(1000);

                var caminhoRaiz = uow.GetRepository<ParametrosSistema>()
                    .QueryFirstOrDefaultAsync(x => x.Parametro == "CAMINHORAIZANEXOS")
                    .Result.Valor;
                string[] caminhoAnexos = new string[anexos.Count];
                string[] nomeAnexos = new string[anexos.Count];
                var i = 0;
                var j = 0;

                foreach (var anexo in anexos)
                {
                    novoAcompanhamento.Anexos.Add(anexo);
                    caminhoAnexos[i++] = caminhoRaiz + anexo.Caminho;
                    nomeAnexos[j++] = anexo.Nome;
                }

                foreach (var venda in vendaCompartilhada)
                {
                    novoAcompanhamento.VendasCompartilhadas.Add(new VendaCompartilhada()
                    {
                        Percentual = venda.Percentual,
                        Produtor = venda.Produtor
                    });
                }

                solicitacao.SituacaoAtual = mapeamento.ProximaSituacao;
                solicitacao.DataHoraSituacaoAtual = novoAcompanhamento.DataEHora;

                bool eFimDeFluxo = solicitacaoAppService.EFimDeFluxo(solicitacao);

                if (((eFimDeFluxo && solicitacao.QtdDiasSLARenovacao == null) || mapeamento.ProximaSituacao.Id.Equals(13) || mapeamento.ProximaSituacao.Id.Equals(46)) && (solicitacao.TipoDeSeguro.Nome.ToLower() == "renovação"))
                {
                    solicitacao.QtdDiasSLARenovacao = solicitacaoAppService.ObterDiasRenovacao(solicitacao.DataFimVigencia ?? DateTime.Now, DateTime.Now);
                }


                var frameCodigoDoBem = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome == "Código do Bem", includes: new[] { "AcoesAcompanhamento" }).Result;

                if (frameCodigoDoBem.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao) && solicitacao.OperacaoDeFinanciamento == 1)
                {
                    if (string.IsNullOrWhiteSpace(codigoDoBem))
                    {
                        throw new ApplicationException("Informar Código(s) do(s) Bem(ns).");
                    }
                    else
                    {
                        foreach (string codigo in codigoDoBem.Split(new char[] { '/' }))
                        {
                            long cod;
                            bool invalido = false;

                            if (!long.TryParse(codigo, out cod))
                                invalido = true;
                            else if (cod <= 0 || codigo.Length > 9)
                                invalido = true;

                            if (invalido)
                                throw new ApplicationException(
                                    "O Código do Bem deve ser numérico e não deve conter mais do que " +
                                    "nove dígitos! É possível informar dois ou mais códigos, separando-os " +
                                    "por barra.");
                        }

                        solicitacao.CodigoDoBem = codigoDoBem;
                    }
                }


                var frameProdutorBnb = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome == "Produtor BNB", includes: new[] { "AcoesAcompanhamento" }).Result;
                if (frameProdutorBnb.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
                {
                    if (produtor == null || produtor <= 0)
                    {
                        if (!solicitacao.Agencia.Nome.StartsWith("999"))
                        {
                            throw new ApplicationException("Informar o produtor BNB");
                        }
                    }
                }

                var frameFinancimento = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome == "Financiamento", includes: new[] { "AcoesAcompanhamento" }).Result;
                if (frameFinancimento.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao) && solicitacao.OperacaoDeFinanciamento == 1)
                {
                    if (string.IsNullOrWhiteSpace(numeroFinanciamento))
                    {
                        throw new ApplicationException("Informar Operação SIAC.");
                    }
                    else
                    {
                        solicitacao.NumeroFinanciamento = caracter.RemoveSpecialCharactersRegex(numeroFinanciamento);
                    }
                }

                var frameCROSSUP = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome == "Projeto CrossUP", includes: new[] { "AcoesAcompanhamento" }).Result;
                if (frameCROSSUP.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
                {
                    if (crossup == null)
                    {
                        throw new ApplicationException("Informar Projeto CrossUP.");
                    }
                    else
                    {
                        solicitacao.CROSSUP = crossup;
                    }
                }

                var frameValorPremio = uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome == "Valor Prêmio", includes: new[] { "AcoesAcompanhamento" }).Result;
                if (frameValorPremio.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
                {
                    if (vlr_premiotot_atual == null)
                    {
                        throw new ApplicationException("Informar Valor Prêmio Líquido Atual.");
                    }
                    else if (perc_comissao_atual == null)
                    {
                        throw new ApplicationException("Informar Percentual Comissão Atual.");
                    }
                    else if (seguradoracotacao == null)
                    {
                        throw new ApplicationException("Informar Seguradora.");
                    }
                    else
                    {
                        solicitacao.vlr_premiotot_atual = vlr_premiotot_atual;
                        solicitacao.perc_comissao_atual = perc_comissao_atual;
                        solicitacao.SeguradoraCotacao = uow.GetRepository<VW_SEGURADORA>().GetByIdAsync(seguradoracotacao.Value).Result;
                    }

                }

                if (frameCapaDaProposta.AcoesAcompanhamento.Any(x => x.AcaoDeAcompanhamento_Id == idDaAcao))
                {
                    if (segmento > 0)
                        solicitacao.Segmento = uow.GetRepository<Segmento>().GetByIdAsync((int)segmento).Result;

                    if (produtor > 0)
                        solicitacao.Produtor = uow.GetRepository<Funcionario>().GetByIdAsync((int)produtor).Result;

                    if (seguradora != null)
                        solicitacao.Seguradora = uow.GetRepository<VW_SEGURADORA>().GetByIdAsync((int)seguradora).Result;


                    if (ramo != null)
                        solicitacao.Ramo = uow.GetRepository<Ramo>().GetByIdAsync((int)ramo).Result;

                    if (!string.IsNullOrWhiteSpace(nu_proposta_seguradora))
                        if (nu_proposta_seguradora.Trim().Length <= 20)
                        {
                            solicitacao.Nu_Proposta_Seguradora = nu_proposta_seguradora;
                        }
                        else
                        {
                            throw new ApplicationException("Número de proposta da seguradora possui mais de 20 digitos, favor verificar.");
                        }

                    if (tipoSeguroGS != null)
                        solicitacao.TipoSeguroGS = uow.GetRepository<TipoDeSeguroGS>().GetByIdAsync((int)tipoSeguroGS).Result;

                    solicitacao.Nu_Apolice_Anterior = nu_apolice_anterior;
                    solicitacao.Pc_comissao = pc_comissao;
                    solicitacao.Co_Corretagem = co_corretagem;
                    solicitacao.Pc_agenciamento = pc_agenciamento;
                    solicitacao.VL_IS = vl_is;

                    if (fl_forma_pagamento_1a != null)
                    {
                        solicitacao.FL_Forma_Pagamento_1a = uow.GetRepository<FormaDePagamento>().GetByIdAsync((int)fl_forma_pagamento_1a).Result;
                    }

                    if (fl_forma_pagamento_demais != null)
                    {
                        solicitacao.FL_Forma_Pagamento_Demais = uow.GetRepository<FormaDePagamento>().GetByIdAsync((int)fl_forma_pagamento_demais).Result;
                    }
                    else
                        solicitacao.FL_Forma_Pagamento_Demais = null;


                    if (grupodeproducao != null)
                    {
                        solicitacao.GrupoDeProducao = uow.GetRepository<GrupoDeProducao>().GetByIdAsync((int)grupodeproducao).Result;
                    }

                    if (tipoDeCategoriaId != null)
                    {
                        solicitacao.TipoDeCategoria = uow.GetRepository<TipoDeCategoria>().GetByIdAsync((int)tipoDeCategoriaId).Result;
                    }

                    if (vlr_premiotot_prop == null)
                    {
                        throw new ApplicationException("Informar Valor Prêmio Líquido fechado.");
                    }
                    else
                    {
                        solicitacao.vlr_premiotot_prop = (decimal)vlr_premiotot_prop;
                    }

                    solicitacao.TipoComissaoRV = TipoComissaoRV;

                    solicitacao.Sede_Envia_Doc_Fisico = sede_envia_doc_fisico;
                    solicitacao.Nu_Sol_Vistoria = nu_sol_vistoria;

                    solicitacao.VIP = vip;

                    solicitacao.Cadastrado_GS = cadastrado_gs;

                    solicitacao.Cd_estudo = cd_estudo;

                    solicitacao.TipoEndosso = tipoEndosso;

                    if (tipoEndosso == "C")
                    {
                        solicitacao.MotivoEndossoCancelamento = uow.GetRepository<MotivoEndossoCancelamento>().QueryFirstOrDefaultAsync(x => x.Id == motivoEndossoCancelamento).Result;
                    }
                }

                if (motivoRecusa != 0)
                {
                    solicitacao.MotivoRecusa = uow.GetRepository<MotivoRecusa>().QueryFirstOrDefaultAsync(x => x.Id == motivoRecusa).Result;
                }

                contextoDeDados.SaveChanges();
                  
                #region Envia E-mail Solicitante Atendente
                if (mapeamento.EnviaEmailSolicitante || mapeamento.EnviaEmailAtendente)
                {
                    var vsObservacao = "";

                    if (novoAcompanhamento.PermiteVisualizarObservacao)
                        vsObservacao = novoAcompanhamento.Observacao;

                    if (solicitacao.Atendente != null && solicitacao.Solicitante != null)
                    {
                        //new MailClient(uow).SendNewAccompanimentMessage(solicitacao.Operador.Email,
                        //                                       solicitacao.Solicitante.Email,
                        //                                       solicitacao.Atendente.Email,
                        //                                       solicitacao.Numero,
                        //                                       mapeamento.SituacaoAtual.Nome,
                        //                                       mapeamento.ProximaSituacao.Nome,
                        //                                       vsObservacao,
                        //                                       solicitacao.Segurado.Nome);
                    }
                }
                #endregion

                #region Envia E-mail de Acompanhamento ao Segurado
                if (mapeamento.EnviaEmailAoSegurado)
                {
                    if (permiteEmailAoSegurado == null || permiteEmailAoSegurado == true)
                    {
                        var vsObservacao = "";

                        if (novoAcompanhamento.PermiteVisualizarObservacao)
                            vsObservacao = novoAcompanhamento.Observacao;

                        if (solicitacao.Atendente != null && solicitacao.Solicitante != null)
                        {
                            var textoPersonalizado = uow.GetRepository<TextoParametrosSistema>()
                                .QueryFirstOrDefaultAsync(x => x.TipoDeProduto.Id == solicitacao.TipoDeProduto.Id &&
                                                                                x.TipoDeSeguro.Id == solicitacao.TipoDeSeguro.Id)
                                .Result.Texto;

                            var textoPorSeguradora = uow.GetRepository<TextoParametroSeguradora>()
                                                                    .QueryFirstOrDefaultAsync(x => x.Seguradora.Id == solicitacao.Seguradora.Id)
                                                                    .Result.Texto;

                            //new MailClient(uow).SendNewAccompanimentSeguradoMessage(solicitacao.Operador.Email,
                            //    solicitacao.Numero,
                            //    solicitacao.Segurado.Nome,
                            //    solicitacao.Atendente.Nome,
                            //    solicitacao.Atendente.Email,
                            //    solicitacao.Segurado.Email,
                            //    caminhoAnexos,
                            //    nomeAnexos,
                            //    vsObservacao,
                            //    mapeamento.ParametrosSistema.Parametro,
                            //    solicitacao.Segurado.EmailSecundario,
                            //    textoPersonalizado,
                            //    textoPorSeguradora);
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MapeamentoAcaoSituacao ObterMapeamentoAcaoSituacao(int idDaSolicitacao, int idDaAcao, List<Grupo> gruposDoUsuario)
        {
            //var situacaoAtual = uow.GetRepository<Solicitacao>().GetByIdAsync(idDaSolicitacao).Result;

            var situacaoAtual = ObterSituacaoAtual(idDaSolicitacao);

            var idsDosGruposDoUsuario = gruposDoUsuario.Select(x => x.Nome);

            var mapeamento = uow.GetRepository<MapeamentoAcaoSituacao>()
                .QueryFirstOrDefaultAsync(x => x.SituacaoAtual.Id == situacaoAtual.Id
                    && idsDosGruposDoUsuario.Any(g => g.IndexOf(x.Grupo.Nome) != -1)
                    && x.Acao.Id == idDaAcao, includes: new[] { "SituacaoAtual","Grupo", "Acao",
                        "ProximaSituacao", "ParametrosSistema", "ParametroSistemaSMS" }).Result;

            return mapeamento;
        }

        private Situacao ObterSituacaoAtual(int idDasolicitacao)
        {
           
            var acompanhamentos = uow.GetRepository<Acompanhamento>()
                .QueryAsync(x => x.Solicitacao.Id == idDasolicitacao,
                includes: new[] { "Situacao" })
                .Result.OrderByDescending(x => x.Ordem).FirstOrDefault();

            if (acompanhamentos.Situacao.Id > 0)
            {
                return acompanhamentos.Situacao;
            }
            else
            {
                throw new ApplicationException("Não existem acompanhamentos cadastrados para a solicitação.");
            }
        }

        private bool ExisteTipoDeCategoriaPorTipoDeProduto(int? tipoDeProduto)
        {
            tipoDeProduto = tipoDeProduto == null ? 0 : tipoDeProduto;

            int categoria = uow.GetRepository<TipoDeCategoria>()
                .QueryAsync(x => x.TipoDeProduto.Id == tipoDeProduto.Value, includes: new[] { "TipoDeProduto" })
                .Result.Count();

            if (categoria > 0)
                return true;
            else
                return false;
        }

        private Acompanhamento AtualizaSLAAcompanhamentoAnterior(Solicitacao solicitacao, DateTime agora)
        {
            Acompanhamento ultimoAcompanhamento = ObterUltimoAcompanhamento(solicitacao);
            bool eParada = ultimoAcompanhamento.Situacao.Tipo.Equals("P") || ultimoAcompanhamento.Situacao.Tipo.Equals("A");
            int totalMinutesSLA = solicitacaoAppService.ObterTempoSLA(ultimoAcompanhamento.DataEHora, agora);
            string tempoSLAEfet = string.Format("{0}h{1:00}m", (int)(totalMinutesSLA / 60), totalMinutesSLA % 60);
            ultimoAcompanhamento.TempoSLAEfet = eParada ? "0h00m" : tempoSLAEfet;

            return ultimoAcompanhamento;
        }
    }
}
