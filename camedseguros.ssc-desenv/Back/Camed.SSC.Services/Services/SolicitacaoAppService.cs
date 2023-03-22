using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.Util;
using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Camed.SSC.Application.ViewModel;
using System.IO;

namespace Camed.SSC.Application.Services
{
    public class SolicitacaoAppService : ISolicitacaoAppService
    {
        private readonly IUnitOfWork uow;
        private readonly IAreaDeNegocioAppService areaDeNegocioAppService;
        private readonly IUsuarioAppService usuarioAppService;
        private readonly ICaracter caracter;
        //private readonly IAcompanhamentoAppService acompanhamentoAppService;
        private readonly IIdentityAppService identity;
        private readonly SSCContext contextoDeDados;

        public SolicitacaoAppService(IUnitOfWork uow, IAreaDeNegocioAppService areaDeNegocioAppService, 
            IUsuarioAppService usuarioAppService, ICaracter caracter, /*
            IAcompanhamentoAppService acompanhamentoAppService,*/ IIdentityAppService identity, 
            SSCContext contextoDeDados)
        {
            this.uow = uow;
            this.areaDeNegocioAppService = areaDeNegocioAppService;
            this.usuarioAppService = usuarioAppService;
            this.caracter = caracter;
            //this.acompanhamentoAppService = acompanhamentoAppService;
            this.identity = identity;
            this.contextoDeDados = contextoDeDados;
        }

        private bool validaCelular(string telefone)
        {
            telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            var regexCelular = new Regex(@"^([1-9]{2})([9]{1})([7-9]{1})([0-9]{7})$");
            return regexCelular.IsMatch(telefone);
        }

        private bool validaEmail(string emailPrincipal)
        {
            var regexEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regexEmail.IsMatch(emailPrincipal);
        }
        
        public void Save(Domain.Entities.Solicitacao entity, List<AnexoDeSolicitacaoViewModel> anexos)
        {
            try
            {
                #region validação do email e celular do segurado
                var email = false;
                var celular = false;
                if (entity.Segurado != null)
                {
                    if (!string.IsNullOrEmpty(entity.Segurado.Email))
                    {
                        email = validaEmail(entity.Segurado.Email);
                    }
                    if (!string.IsNullOrEmpty(entity.Segurado.EmailSecundario) && !email)
                    {
                        email = validaEmail(entity.Segurado.EmailSecundario);
                    }

                    if (!string.IsNullOrEmpty(entity.Segurado.TelefonePrincipal))
                    {
                        celular = validaCelular(entity.Segurado.TelefonePrincipal);
                    }
                    if (!string.IsNullOrEmpty(entity.Segurado.TelefoneCelular) && !celular)
                    {
                        celular = validaCelular(entity.Segurado.TelefoneCelular);
                    }
                    if (!string.IsNullOrEmpty(entity.Segurado.TelefoneAdicional) && !celular)
                    {
                        celular = validaCelular(entity.Segurado.TelefoneAdicional);
                    }
                }

                if (!email && !celular)
                {
                    throw new ApplicationException("O segurado não possui um e-mail ou um celular válido cadastrado. Favor corrigir o cadastro.");
                }
                #endregion

                bool eRenovacao = uow.GetRepository<TipoDeSeguro>()
                    .QueryAsync(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "renovação").Result.Any();

                bool eProspeccao = uow.GetRepository<TipoDeSeguro>()
                    .QueryAsync(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "prospecção").Result.Any();

                bool tipoDeSeguroETipoDeProdutoAssociados = uow.GetRepository<TipoDeSeguro>()
                    .QueryAsync(x => x.Id == entity.TipoDeSeguro.Id, includes: new[] { "TiposDeProduto" }).Result
                    .Any(w => w.TiposDeProduto.Any(p => p.TipoDeProduto.Id == entity.TipoDeProduto.Id && p.TipoDeProduto.Ativo == true));

                if (!tipoDeSeguroETipoDeProdutoAssociados)
                {
                    throw new ApplicationException("O ramo de seguro e o tipo de seguro informados não estão associados.");
                }

                if (!uow.GetRepository<Solicitacao>().QueryAsync(s => s.Numero == entity.Numero).Result.Any())
                {
                    DoCustomValidation(entity, true);

                    DateTime agora = DateTime.Now;
                    Situacao situacaoInicial = new Situacao();
                    if (eRenovacao)
                    {
                        situacaoInicial = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(p => p.Id ==
                        entity.TipoDeProduto.Id, includes: new[] { "Situacao", "SituacaoRenovacao" }).Result.SituacaoRenovacao;
                    }
                    else if (eProspeccao)
                    {
                        situacaoInicial = uow.GetRepository<Situacao>().QueryFirstOrDefaultAsync(p => p.Nome.Equals("Prospecção")).Result;
                    }
                    else
                    {
                        situacaoInicial = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(p => p.Id ==
                        entity.TipoDeProduto.Id, "Situacao").Result.Situacao;
                    }
                    var novaSolicitacao = new Solicitacao()
                    {
                        Numero = entity.Numero,
                        DataDeIngresso = agora,
                        AreaDeNegocio = areaDeNegocioAppService.RetornarAreaNegocio(entity),
                        SituacaoAtual = situacaoInicial,
                        DataHoraSituacaoAtual = agora,
                        Origem = entity.Origem,
                        OperacaoDeFinanciamento = entity.OperacaoDeFinanciamento,
                        OrcamentoPrevio = entity.OrcamentoPrevio,
                        Mercado = entity.Mercado,
                        CROSSUP = entity.CROSSUP,
                        NumeroFinanciamento = entity.NumeroFinanciamento,
                        CodigoDoBem = entity.CodigoDoBem,
                        Agencia = uow.GetRepository<Agencia>().QueryFirstOrDefaultAsync(w => w.Id == entity.Agencia.Id).Result,
                        DadosAdicionais = entity.DadosAdicionais,
                        Operador = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == entity.Operador.Id).Result,
                        TipoDeProduto = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(w => w.Id == entity.TipoDeProduto.Id).Result,
                        TipoDeSeguro = uow.GetRepository<TipoDeSeguro>().QueryFirstOrDefaultAsync(w => w.Id == entity.TipoDeSeguro.Id).Result,
                        Atendente = entity.Atendente == null ? null : uow.GetRepository<Usuario>().GetByIdAsync(entity.Atendente.Id).Result,
                        DataFimVigencia = entity.DataFimVigencia,
                        CanalDeDistribuicao = entity.CanalDeDistribuicao == null ? null : uow.GetRepository<CanalDeDistribuicao>()
                        .GetByIdAsync(entity.CanalDeDistribuicao.Id).Result,
                        estudo_origem = entity.estudo_origem,

                    };

                    if (entity.Atendente == null)
                    {
                        novaSolicitacao.Atendente = usuarioAppService.RetornarAtendente(entity);
                    }

                    if (entity.Aplicacao != null)
                    {
                        novaSolicitacao.Aplicacao = entity.Aplicacao;
                    }

                    if (entity.AgenciaConta != null)
                    {
                        novaSolicitacao.AgenciaConta = entity.AgenciaConta;
                    }

                    if (entity.Segmento != null)
                    {
                        novaSolicitacao.Segmento = entity.Segmento;
                    }

                    if (entity.Produtor != null)
                    {
                        novaSolicitacao.Produtor = entity.Produtor;
                    }

                    if (entity.Ramo != null)
                    {
                        novaSolicitacao.Ramo = entity.Ramo;
                    }

                    var documentos = uow.GetRepository<TipoDeDocumento>().QueryAsync(x =>
                    x.TiposDeProduto.Any(p => p.TipoDeProduto.Id == entity.TipoDeProduto.Id)).Result.ToList();

                    foreach (var doc in documentos)
                    {
                        novaSolicitacao.CheckList.Add(new SolCheckList()
                        {
                            Id = 0,
                            DocumentoAnexado = true,
                            TipoDeDocumento = doc
                        });
                    }

                    foreach (var ind in entity.Indicacoes)
                    {
                        novaSolicitacao.Indicacoes.Add(new SolicitacaoIndicacoes()
                        {
                            Id = 0,
                            Nome = ind.Nome,
                            Telefone = ind.Telefone
                        });
                    }

                    var grupo = uow.GetRepository<Grupo>().QueryFirstOrDefaultAsync(x => x.Nome.Equals("Atendente")).Result;

                    var operador = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(u => u.Id == entity.Operador.Id).Result;

                    Acompanhamento acompanhamento = new Acompanhamento();
                    acompanhamento.Ordem = entity.Acompanhamentos.Count() + 1;
                    acompanhamento.DataEHora = agora;
                    acompanhamento.Situacao = situacaoInicial;
                    acompanhamento.Atendente = operador;
                    acompanhamento.Grupo = grupo;
                    acompanhamento.TempoSLADef = situacaoInicial.TempoSLA;
                    if (eProspeccao)
                    {
                        acompanhamento.TempoSLAEfet = "0h00m";
                    }
                    else
                    {
                        acompanhamento.TempoSLAEfet = null;
                    }

                    novaSolicitacao.Acompanhamentos.Add(acompanhamento);

                    entity.Segurado.TelefonePrincipal = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefonePrincipal);
                    entity.Segurado.TelefoneCelular = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefoneCelular);
                    entity.Segurado.TelefoneAdicional = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefoneAdicional);
                    entity.Segurado.CpfCnpj = caracter.RemoveSpecialCharacter2(entity.Segurado.CpfCnpj).Replace("/", "");

                    if (string.IsNullOrWhiteSpace(entity.Segurado.Email) || entity.Segurado.Email.ToLower().Trim() == "null")
                    {
                        entity.Segurado.Email = null;
                    }

                    if (string.IsNullOrWhiteSpace(entity.Segurado.EmailSecundario) || entity.Segurado.EmailSecundario.ToLower().Trim() == "null")
                    {
                        entity.Segurado.EmailSecundario = null;
                    }

                    var segurado = new Segurado()
                    {
                        TelefonePrincipal = entity.Segurado.TelefonePrincipal,
                        TelefoneCelular = entity.Segurado.TelefoneCelular,
                        TelefoneAdicional = entity.Segurado.TelefoneAdicional,
                        CpfCnpj = entity.Segurado.CpfCnpj,
                        Email = entity.Segurado.Email,
                        Nome = entity.Segurado.Nome,
                        EmailSecundario = entity.Segurado.EmailSecundario,
                        Contato = entity.Segurado.Contato
                    };

                    if (entity.Segurado.VinculoBNB != null)
                    {
                        segurado.VinculoBNB = entity.Segurado.VinculoBNB;
                    }

                    novaSolicitacao.Segurado = segurado;

                    entity.Solicitante.TelefonePrincipal = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefonePrincipal);
                    entity.Solicitante.TelefoneCelular = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefoneCelular);
                    entity.Solicitante.TelefoneAdicional = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefoneAdicional);

                    var solicitante = new Solicitante()
                    {
                        TelefonePrincipal = entity.Solicitante.TelefonePrincipal,
                        TelefoneCelular = entity.Solicitante.TelefoneCelular,
                        TelefoneAdicional = entity.Solicitante.TelefoneAdicional,
                        Email = entity.Solicitante.Email,
                        Nome = entity.Solicitante.Nome,
                        Usuario = uow.GetRepository<Usuario>().GetByIdAsync(entity.Solicitante.Usuario.Id).Result
                    };

                    novaSolicitacao.Solicitante = solicitante;

                    uow.GetRepository<Solicitacao>().AddAndSaveAsync(novaSolicitacao).Wait();


                    if (anexos.Count > 0)
                    {
                        SaveFile(anexos, uow.GetRepository<Solicitacao>().QueryFirstOrDefaultAsync(w => w.Numero == novaSolicitacao.Numero).Result.Id);
                    }

                    uow.Commit();
                }
                else
                {
                    DoCustomValidation(entity, false);

                    var solicitacaoOriginal = uow.GetRepository<Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == entity.Id,
                        includes: new[] { "Atendente", "Segurado", "Agencia", "AgenciaConta", "CheckList", "Indicacoes",
                            "TipoDeProduto", "TipoDeSeguro", "SituacaoAtual", "TipoDeProduto", "TipoDeProduto.Situacao", "Solicitante", "Segmento", }).Result;

                    bool alterouRamoDeSeguro = entity.TipoDeProduto.Id != solicitacaoOriginal.TipoDeProduto.Id;

                    solicitacaoOriginal.TipoDeProduto.Id = entity.TipoDeProduto.Id;

                    if (entity.Produtor != null)
                    {
                        solicitacaoOriginal.Produtor.Id = entity.Produtor.Id;
                    }

                    if (entity.Atendente != null)
                    {
                        if (alterouRamoDeSeguro)
                        {
                            bool existeRegistroSolicitacaoAtendente = ExisteRegistroSolicitacaoAtendente(entity.Id, entity.Atendente.Id);

                            bool solicitacaoJaEmAtendimento = SolicitacaoJaEmAtendimento(entity.Id);

                            if (existeRegistroSolicitacaoAtendente && solicitacaoJaEmAtendimento)
                            {
                                throw new ApplicationException("Você não pode modificar o Ramo de Seguro desta solicitação, já em atendimento.");
                            }

                            solicitacaoOriginal.Atendente = null;

                            Situacao situacaoInicialNova = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(x => x.Id ==
                            entity.TipoDeProduto.Id, includes: new[] { "Situacao" }).Result.Situacao;

                            if (solicitacaoOriginal.TipoDeProduto.Situacao.Id != situacaoInicialNova.Id)
                            {
                                var acompanhamento = ObterUltimoAcompanhamento(entity);
                                acompanhamento.Situacao = situacaoInicialNova;
                                solicitacaoOriginal.SituacaoAtual = situacaoInicialNova;
                            }
                        }
                    }

                    solicitacaoOriginal.Acompanhamentos = ObterAcompanhamentos(entity.Id);
                    solicitacaoOriginal.AreaDeNegocio = areaDeNegocioAppService.RetornarAreaNegocio(entity);
                    solicitacaoOriginal.DataHoraSituacaoAtual = ObterUltimoAcompanhamento(entity).DataEHora;

                    if (alterouRamoDeSeguro)
                    {
                        foreach (var checklist in solicitacaoOriginal.CheckList)
                        {
                            uow.GetRepository<SolCheckList>().Remove(checklist);
                        }

                        var documentos = uow.GetRepository<TipoDeDocumento>().QueryAsync(x =>
                        x.TiposDeProduto.Any(p => p.TipoDeDocumento.Id == entity.TipoDeProduto.Id)).Result
                            .ToList();

                        var novaCheckList = new List<SolCheckList>();

                        foreach (var doc in documentos)
                        {
                            solicitacaoOriginal.CheckList.Add(new SolCheckList()
                            {
                                Id = 0,
                                DocumentoAnexado = false,
                                DocumentoAnexadoConfirmado = false,
                                TipoDeDocumento = uow.GetRepository<TipoDeDocumento>().GetByIdAsync(doc.Id).Result
                            });
                        }
                    }

                    bool alterouEmailSolicitante = entity.Solicitante.Email != solicitacaoOriginal.Solicitante.Email;

                    if (alterouEmailSolicitante)
                    {
                        solicitacaoOriginal.Solicitante.Email = entity.Solicitante.Email;
                    }

                    bool alterouTelefonePrincipalDoSolicitante = entity.Solicitante.TelefonePrincipal != solicitacaoOriginal.Solicitante.TelefonePrincipal;

                    if (alterouTelefonePrincipalDoSolicitante)
                    {
                        solicitacaoOriginal.Solicitante.TelefonePrincipal = entity.Solicitante.TelefonePrincipal;
                    }

                    bool alterouCpfCnpjSegurado = entity.Segurado.CpfCnpj != solicitacaoOriginal.Segurado.CpfCnpj;
                    if (alterouCpfCnpjSegurado)
                    {
                        solicitacaoOriginal.Segurado.CpfCnpj = caracter.RemoveSpecialCharacter2(entity.Segurado.CpfCnpj.Trim()).Replace("/", "");
                    }

                    bool alterouNomeSegurado = entity.Segurado.Nome != solicitacaoOriginal.Segurado.Nome;
                    if (alterouNomeSegurado)
                    {
                        solicitacaoOriginal.Segurado.Nome = entity.Segurado.Nome;
                    }

                    bool alterouEmailSegurado = entity.Segurado.Email != solicitacaoOriginal.Segurado.Email;
                    if (alterouEmailSegurado)
                    {
                        solicitacaoOriginal.Segurado.Email = entity.Segurado.Email;
                    }

                    bool alterouEmailSeguradoSecundario = entity.Segurado.EmailSecundario != solicitacaoOriginal.Segurado.EmailSecundario;

                    if (alterouEmailSeguradoSecundario)
                    {
                        solicitacaoOriginal.Segurado.EmailSecundario = entity.Segurado.EmailSecundario;
                    }

                    bool alterouTelefonePrincipal = entity.Segurado.TelefonePrincipal != solicitacaoOriginal.Segurado.TelefonePrincipal;

                    if (alterouTelefonePrincipal)
                    {
                        solicitacaoOriginal.Segurado.TelefonePrincipal = entity.Segurado.TelefonePrincipal;
                    }

                    bool alterouCelular = entity.Segurado.TelefoneCelular != solicitacaoOriginal.Segurado.TelefoneCelular;

                    if (alterouCelular)
                    {
                        solicitacaoOriginal.Segurado.TelefoneCelular = entity.Segurado.TelefoneCelular;
                    }

                    bool alterouOutro = entity.Segurado.TelefoneAdicional != solicitacaoOriginal.Segurado.TelefoneAdicional;

                    if (alterouOutro)
                    {
                        solicitacaoOriginal.Segurado.TelefoneAdicional = entity.Segurado.TelefoneAdicional;
                    }

                    bool alterouContato = entity.Segurado.Contato != solicitacaoOriginal.Segurado.Contato;

                    if (alterouContato)
                    {
                        solicitacaoOriginal.Segurado.Contato = entity.Segurado.Contato;
                    }

                    if (entity.Segmento != null)
                    {
                        solicitacaoOriginal.Segmento = entity.Segmento;
                    }

                    if (anexos.Count > 0)
                    {
                        SaveFile(anexos, solicitacaoOriginal.Id);
                    }

                    uow.CommitAsync();
                }
            } 
            catch(Exception e)
            {
                throw e;
            }
        }

        private async void SaveFile(List<AnexoDeSolicitacaoViewModel> Anexos, int Solicitacao_Id)
        {
            var solicitacao = uow.GetRepository<Solicitacao>().QueryFirstOrDefaultAsync(w => w.Id == Solicitacao_Id, includes: "Anexos").Result;

            foreach (var anexo in Anexos)
            {
                anexo.Solicitacao_Id = Solicitacao_Id;
                var caminhoDiretorioAnexos = (uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS").Result).Valor;
                caminhoDiretorioAnexos += $"\\anexos\\solicitacao\\{solicitacao.Agencia.Codigo}\\{solicitacao.Numero}";

                if (!Directory.Exists(caminhoDiretorioAnexos))
                {
                    Directory.CreateDirectory(caminhoDiretorioAnexos);
                }

                var caminhoFisicoAnexo = $"{caminhoDiretorioAnexos}\\{anexo.Nome}{anexo.Extensao}";
                anexo.Caminho = $"\\anexos\\solicitacao\\{solicitacao.Agencia.Codigo}\\{solicitacao.Numero}\\{ anexo.Nome}{anexo.Extensao}";

                if (File.Exists(anexo.Caminho))
                {
                    var novoNome = $"{ anexo.Nome }{ Path.GetRandomFileName()}";
                    anexo.Caminho = $"{caminhoDiretorioAnexos}\\{novoNome}{ anexo.Extensao}";
                    anexo.Nome = novoNome + anexo.Extensao;
                }
                else
                {
                    anexo.Nome = anexo.Nome + anexo.Extensao;
                }

                var novoAnexo = new AnexoDeSolicitacao();
                novoAnexo.Caminho = anexo.Caminho;
                novoAnexo.Nome = anexo.Nome;
                novoAnexo.Solicitacao = solicitacao;

                solicitacao.Anexos.Add(novoAnexo);

                using (var stream = new FileStream(caminhoFisicoAnexo, FileMode.Create))
                {
                    stream.Write(anexo.Arquivo);
                }
            }
            uow.GetRepository<Solicitacao>().UpdateAndSaveAsync(solicitacao).Wait();
        }

        private void DoCustomValidation(Solicitacao entity, bool novo)
        {
            if (entity.TipoDeSeguro == null)
            {
                throw new ApplicationException("Tipo de Seguro deve ser informado");
            }

            bool eSinistro = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower().IndexOf("sinistro") >= 0);


            bool eRenovacao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "renovação");


            bool eProspeccao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "prospecção");


            bool eRecontratacao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "recontratação");

            if (novo)
            {
                if (eSinistro)
                {
                    if (entity.OperacaoDeFinanciamento != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Operação de Financiamento não deve ser preenchido");
                    }
                    if (entity.OrcamentoPrevio != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Orçamento Prévio não deve ser preenchido");
                    }

                    if (entity.Mercado != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Mercadonão deve ser preenchido");
                    }
                    else if (entity.Segmento != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Segmento não deve ser preenchido");
                    }
                    else if (entity.Segurado.VinculoBNB != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Vínculo BNB não deve ser preenchido");
                    }

                    else if (entity.DataFimVigencia != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Data Fim Vigência não deve ser preenchido");
                    }
                }
                else
                {
                    if (entity.OperacaoDeFinanciamento == null)
                    {
                        throw new ApplicationException("Operação de Financiamento deve ser informado");
                    }
                    else if (!eRenovacao && entity.Segmento == null && entity.Origem != 3 /* Web */)
                    {
                        throw new ApplicationException("Segmento deve ser informado");
                    }
                    else if (entity.Segurado.VinculoBNB == null)
                    {
                        throw new ApplicationException("Vínculo BNB deve ser informado");
                    }

                    else if (entity.AgenciaConta == null)
                    {
                        throw new ApplicationException("Agencia Conta deve ser informada");
                    }


                    if ((eRenovacao || eProspeccao) && (entity.DataFimVigencia == null))
                    {
                        throw new ApplicationException("Data Fim Vigência deve ser informado");
                    }

                }
            }
            else
            {
                if (eSinistro)
                {
                    if (entity.OperacaoDeFinanciamento != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Operação de Financiamento não deve ser preenchido");
                    }

                    else if (entity.Segmento != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Segmento não deve ser preenchido");
                    }
                    else if (entity.Segurado.VinculoBNB != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Vínculo BNB não deve ser preenchido");
                    }

                    else if (entity.DataFimVigencia != null)
                    {
                        throw new ApplicationException(
                            "Para solicitações do tipo Sinistro, o campo Data Fim Vigência não deve ser preenchido");
                    }
                }
                else
                {
                    if (entity.OperacaoDeFinanciamento == null)
                    {
                        throw new ApplicationException("Operação de Financiamento deve ser informado");
                    }
                    else if (!eRenovacao && entity.Segmento == null && entity.Origem != 3 /* Web */)
                    {
                        throw new ApplicationException("Segmento deve ser informado");
                    }
                    else if (entity.Segurado.VinculoBNB == null)
                    {
                        throw new ApplicationException("Vínculo BNB deve ser informado");

                    }
                    if ((eRenovacao || eProspeccao) && (entity.DataFimVigencia == null))
                    {
                        throw new ApplicationException("Data Fim Vigência deve ser informado");
                    }
                }
            }
        }

        public bool ExisteRegistroSolicitacaoAtendente(int idSolicitacao, int idAtendente)
        {
            return uow.GetRepository<SolicitacaoMovimentacaoRamo>().QueryAsync(x => x.Solicitacao.Id == idSolicitacao && x.Atendente.Id == idAtendente).Result.Any();
        }

        public bool SolicitacaoJaEmAtendimento(int idSolicitacao)
        {
            return uow.GetRepository<Acompanhamento>().QueryAsync(x => x.Solicitacao.Id == idSolicitacao).Result.Count() > 1 ? true : false;
        }

        public ICollection<Acompanhamento> ObterAcompanhamentos(int idDaSolicitacao)
        {
            var solicitacao = uow.GetRepository<Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == idDaSolicitacao, 
                includes: new[] { "Acompanhamentos" }).Result;

            if (solicitacao == null)
                throw new ApplicationException("Não foi encontrada solicitação com o identificador informado.");

            return solicitacao.Acompanhamentos;
        }

        public void ValidarSolicitacao(Solicitacao solicitacao)
        {
            if (solicitacao.OperacaoDeFinanciamento != 1)
                throw new ApplicationException("Operação de financiamento inválida.");

            if (string.IsNullOrEmpty(solicitacao.DadosAdicionais) || string.IsNullOrWhiteSpace(solicitacao.DadosAdicionais))
                throw new ApplicationException("O campo 'DadosAdicionais' não pode ser vazio ou em branco.");

            if (solicitacao.TipoDeSeguro.Id != 1)
                throw new ApplicationException("Tipo de seguro inválido.");

            if (solicitacao.CanalDeDistribuicao.Id != 1 && solicitacao.CanalDeDistribuicao.Id != 3)
                throw new ApplicationException("Canal de distribuição inválido");

            if (solicitacao.CanalDeDistribuicao.Id == 1)
            {
                var produtosBNB = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 32 };

                var produtoEncontrado = VerificarTipoDeProduto(produtosBNB, solicitacao.TipoDeProduto.Id);

                if (!produtoEncontrado)
                    throw new ApplicationException("Produto não encontrado para o canal de distribuição informado.");
            }

            if (solicitacao.CanalDeDistribuicao.Id == 3)
            {
                var produtosAgrosul = new int[] { 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };

                var produtoEncontrado = VerificarTipoDeProduto(produtosAgrosul, solicitacao.TipoDeProduto.Id);

                if (!produtoEncontrado)
                    throw new ApplicationException("Produto não encontrado para o canal de distribuição informado.");
            }

            if (solicitacao.Segurado.VinculoBNB.Id != 1 && solicitacao.Segurado.VinculoBNB.Id != 2)
                throw new ApplicationException("Vinculo BNB inválido.");

            if (solicitacao.OrcamentoPrevio != 0 && solicitacao.OrcamentoPrevio != 1)
                throw new ApplicationException("Orçamento prévio inválido.");

            ValidarCodigoDoBem(solicitacao.CodigoDoBem);

        }

        public void SaveCopy(Solicitacao entity, string login = null)
        {
            bool tipoDeSeguroETipoDeProdutoAssociados = uow.GetRepository<TipoDeSeguro>().QueryAsync(x => x.Id == entity.TipoDeSeguro.Id, includes: new[]{
            "TipoDeProduto"
            }).Result.Any(x => x.TiposDeProduto.Any(p => p.TipoDeProduto.Id == entity.TipoDeProduto.Id && p.TipoDeProduto.Ativo == true));

            if (!tipoDeSeguroETipoDeProdutoAssociados)
            {
                throw new ApplicationException("O ramo de seguro e o tipo de seguro informados não estão associados.");
            }

            Situacao situacaoInicCopia = uow.GetRepository<Acompanhamento>().QueryFirstOrDefaultAsync(x => x.Solicitacao.Numero == entity.Numero 
            && x.Ordem == 1, includes: new[] { "Solicitacao", "Situacao" }).Result.Situacao;

            AreaDeNegocio areaNegocioInicCopia = uow.GetRepository<AreaDeNegocio>().QueryFirstOrDefaultAsync(x => x.Id == entity.AreaDeNegocio.Id).Result;

            entity.Numero = GenerateSequentialNumber(login);
            DateTime agora = DateTime.Now;

            var novaSolicitacao = new Solicitacao()
            {
                Numero = entity.Numero,
                DataDeIngresso = agora,
                AreaDeNegocio = areaNegocioInicCopia,
                SituacaoAtual = situacaoInicCopia,
                DataHoraSituacaoAtual = agora,
                Origem = entity.Origem,
                OperacaoDeFinanciamento = entity.OperacaoDeFinanciamento,
                OrcamentoPrevio = entity.OrcamentoPrevio,
                Mercado = entity.Mercado,
                NumeroFinanciamento = entity.NumeroFinanciamento,
                CodigoDoBem = entity.CodigoDoBem,
                Agencia = uow.GetRepository<Agencia>().QueryFirstOrDefaultAsync(w => w.Id == entity.Agencia.Id).Result,
                DadosAdicionais = entity.DadosAdicionais,
                Operador = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == entity.Operador.Id).Result,
                TipoDeProduto = uow.GetRepository<TipoDeProduto>().QueryFirstOrDefaultAsync(w => w.Id == entity.TipoDeProduto.Id).Result,
                TipoDeSeguro = uow.GetRepository<TipoDeSeguro>().QueryFirstOrDefaultAsync(w => w.Id == entity.TipoDeSeguro.Id).Result,
                DataFimVigencia = entity.DataFimVigencia,
                vlr_premiotot_anterior = entity.vlr_premiotot_anterior,
                perc_comissao_anterior = entity.perc_comissao_anterior,
                estudo_origem = entity.estudo_origem == null ? null : entity.estudo_origem,
                CanalDeDistribuicao = entity.CanalDeDistribuicao != null ? uow.GetRepository<CanalDeDistribuicao>()
                .QueryFirstOrDefaultAsync(w => w.Id == entity.CanalDeDistribuicao.Id).Result : null
            };

            if (entity.AgenciaConta != null)
            {
                novaSolicitacao.AgenciaConta = entity.AgenciaConta;
            }

            if (entity.Aplicacao != null)
            {
                novaSolicitacao.Aplicacao = entity.Aplicacao;
            }

            if (entity.Atendente != null)
            {
                var atendente = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(f => f.Id == entity.Atendente.Id).Result;
                novaSolicitacao.Atendente = atendente;
            }

            if (entity.Segmento != null)
            {
                novaSolicitacao.Segmento = entity.Segmento;
            }

            if (entity.Produtor != null)
            {
                novaSolicitacao.Produtor = entity.Produtor;
            }

            if (entity.Ramo != null)
            {
                novaSolicitacao.Ramo = entity.Ramo;
            }

            var documentos = uow.GetRepository<TipoDeDocumento>().QueryAsync(x => x.TiposDeProduto.Any(p => p.TipoDeProduto.Id == entity.TipoDeProduto.Id))
                .Result.ToList();

            foreach (var doc in documentos)
            {
                novaSolicitacao.CheckList.Add(new SolCheckList()
                {
                    Id = 0,
                    DocumentoAnexado = false,
                    DocumentoAnexadoConfirmado = false,
                    TipoDeDocumento = doc
                });
            }

            foreach (var ind in entity.Indicacoes)
            {
                novaSolicitacao.Indicacoes.Add(new SolicitacaoIndicacoes()
                {
                    Id = 0,
                    Nome = ind.Nome,
                    Telefone = ind.Telefone
                });
            }

            var grupo = uow.GetRepository<Grupo>().QueryFirstOrDefaultAsync(x => x.Nome.Equals("Atendente")).Result;

            var operador = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(u => u.Id == entity.Operador.Id).Result;

            Acompanhamento acompanhamento = new Acompanhamento();
            acompanhamento.Ordem = entity.Acompanhamentos.Count() + 1;
            acompanhamento.DataEHora = agora;
            acompanhamento.Situacao = situacaoInicCopia;
            acompanhamento.Atendente = operador;
            acompanhamento.Grupo = grupo;
            acompanhamento.TempoSLADef = situacaoInicCopia.TempoSLA;
            acompanhamento.TempoSLAEfet = null;

            novaSolicitacao.Acompanhamentos.Add(acompanhamento);

            entity.Segurado.TelefonePrincipal = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefonePrincipal);
            entity.Segurado.TelefoneCelular = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefoneCelular);
            entity.Segurado.TelefoneAdicional = caracter.RemoveSpecialCharacter2(entity.Segurado.TelefoneAdicional);
            entity.Segurado.CpfCnpj = caracter.RemoveSpecialCharacter2(entity.Segurado.CpfCnpj).Replace("/", "");

            if (string.IsNullOrWhiteSpace(entity.Segurado.Email) || entity.Segurado.Email.ToLower().Trim() == "null")
            {
                entity.Segurado.Email = null;
            }

            if (string.IsNullOrWhiteSpace(entity.Segurado.EmailSecundario) || entity.Segurado.EmailSecundario.ToLower().Trim() == "null")
            {
                entity.Segurado.EmailSecundario = null;
            }

            var segurado = new Segurado()
            {
                TelefonePrincipal = entity.Segurado.TelefonePrincipal,
                TelefoneCelular = entity.Segurado.TelefoneCelular,
                TelefoneAdicional = entity.Segurado.TelefoneAdicional,
                CpfCnpj = entity.Segurado.CpfCnpj,
                Email = entity.Segurado.Email,
                Nome = entity.Segurado.Nome,
                EmailSecundario = entity.Segurado.EmailSecundario,
                Contato = entity.Segurado.Contato
            };

            if (entity.Segurado.VinculoBNB != null)
            {
                segurado.VinculoBNB.Id = entity.Segurado.VinculoBNB.Id;
            }

            novaSolicitacao.Segurado = segurado;

            entity.Solicitante.TelefonePrincipal = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefonePrincipal);
            entity.Solicitante.TelefoneCelular = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefoneCelular);
            entity.Solicitante.TelefoneAdicional = caracter.RemoveSpecialCharacter2(entity.Solicitante.TelefoneAdicional);

            var solicitante = new Solicitante()
            {
                TelefonePrincipal = entity.Solicitante.TelefonePrincipal,
                TelefoneCelular = entity.Solicitante.TelefoneCelular,
                TelefoneAdicional = entity.Solicitante.TelefoneAdicional,
                Email = entity.Solicitante.Email,
                Nome = entity.Solicitante.Nome,
                Usuario = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == entity.Solicitante.Usuario.Id).Result
            };

            if (novaSolicitacao.TipoDeSeguro.Id == 2)
            {
                if (novaSolicitacao.DataFimVigencia < DateTime.Now.AddDays(-60))
                {
                    novaSolicitacao.TipoDeSeguro.Id = 1;
                    novaSolicitacao.DataFimVigencia = null;

                    situacaoInicCopia = uow.GetRepository<TipoDeProduto>()
                        .QueryFirstOrDefaultAsync(p => p.Id == entity.TipoDeProduto.Id, includes: new[] { "Situacao" }).Result.Situacao;

                    novaSolicitacao.SituacaoAtual = situacaoInicCopia;

                }
            }

            novaSolicitacao.Solicitante = solicitante;

            uow.GetRepository<Solicitacao>().AddAndSaveAsync(novaSolicitacao);

            uow.CommitAsync();
        }

        private int GenerateSequentialNumber(string login = null)
        {
            var lg = login ?? uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == identity.Identity.Id).Result.Nome;

            SequencialDeSolicitacao ultimoId = new SequencialDeSolicitacao();
            ultimoId.Operador = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Nome == lg).Result;
            ultimoId.Data = DateTime.Now;

            uow.GetRepository<SequencialDeSolicitacao>().AddAndSaveAsync(ultimoId);
            uow.CommitAsync();

            return ultimoId.Id;
        }

        private bool VerificarTipoDeProduto(int[] items, int produtoId)
        {
            var produtoEncontrado = false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == produtoId)
                    produtoEncontrado = true;
            }

            return produtoEncontrado;
        }

        private void ValidarCodigoDoBem(string codigo)
        {
            var codigos = codigo.Split('/');

            foreach (var item in codigos)
            {
                if (item.Length > 9)
                {
                    throw new ApplicationException("O código: " + item + " deve conter apenas 9 dígitos.");
                }
            }
        }

        public int ObterNumero(int idDaSolicitacao)
        {
            var solicitacao = uow.GetRepository<Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == 
            idDaSolicitacao).Result;

            if (solicitacao == null)
                throw new ApplicationException("Não foi encontrada solicitação com o identificador informado.");

            return solicitacao.Numero;
        }

        public int ObterTempoSLA(DateTime inicio, DateTime fim)
        {
            try
            {
                //A testar
                var ret = contextoDeDados.Database.ExecuteSqlCommand("SELECT [DBO].[CS_GETHORASSLA] (@p1, @p2)", new SqlParameter("@p1", inicio), new SqlParameter("@p2", fim));

                return ret;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool EFimDeFluxo(Solicitacao solicitacao)
        {
            return uow.GetRepository<Situacao>().QueryAsync(x => x.EFimFluxo == true 
            && x.Id == solicitacao.SituacaoAtual.Id, includes: new[] { "Id" }).Result.Any();
        }

        public int ObterDiasRenovacao(DateTime inicio, DateTime fim)
        {
            try
            {
                var qtddias = (int)inicio.Date.Subtract(fim.Date).TotalDays;

                return qtddias;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Acompanhamento ObterUltimoAcompanhamento(Solicitacao solicitacao)
        {
            var acompanhamento = uow.GetRepository<Acompanhamento>().QueryAsync(x => x.Solicitacao.Id == solicitacao.Id,
                includes: new[] { "Anexos", "Atendente", "Grupo", "Solicitacao", "Situacao", "VendasCompartilhadas" })
                .Result.OrderByDescending(w => w.DataEHora).FirstOrDefault();

            return acompanhamento;
        }

        /*private void DoCustomValidation(Solicitacao entity, bool novo)
        {
            if (entity.TipoDeSeguro == null)
            {
                ThrowValidationError("Tipo de Seguro", "Tipo de Seguro deve ser informado");
            }

            bool eSinistro = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower().IndexOf("sinistro") >= 0);

            bool eRenovacao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "renovação");

            bool eProspeccao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "prospecção");

            bool eRecontratacao = contextoDeDados.TiposDeSeguro
                    .Any(x => x.Id == entity.TipoDeSeguro.Id && x.Nome.ToLower() == "recontratação");

            if (novo)
            {
                if (eSinistro)
                {
                    if (entity.OperacaoDeFinanciamento != null)
                    {
                        throw new AplicationException("Operação de Financiamento",
                            "Para solicitações do tipo Sinistro, o campo Operação de Financiamento não deve ser preenchido");
                    }
                    if (entity.OrcamentoPrevio != null)
                    {
                        ThrowValidationError("Orçamento Prévio",
                            "Para solicitações do tipo Sinistro, o campo Orçamento Prévio não deve ser preenchido");
                    }

                    if (entity.Mercado != null)
                    {
                        ThrowValidationError("Mercado",
                            "Para solicitações do tipo Sinistro, o campo Mercadonão deve ser preenchido");
                    }
                    else if (entity.Segmento != null)
                    {
                        ThrowValidationError("Segmento",
                            "Para solicitações do tipo Sinistro, o campo Segmento não deve ser preenchido");
                    }
                    else if (entity.Segurado.VinculoBNB != null)
                    {
                        ThrowValidationError("Vínculo BNB",
                            "Para solicitações do tipo Sinistro, o campo Vínculo BNB não deve ser preenchido");
                    }
                    else if (entity.DataFimVigencia != null)
                    {
                        ThrowValidationError("Data Fim Vigência",
                            "Para solicitações do tipo Sinistro, o campo Data Fim Vigência não deve ser preenchido");
                    }
                }
                else
                {
                    if (entity.OperacaoDeFinanciamento == null)
                    {
                        ThrowValidationError("Operação de Financiamento", "Operação de Financiamento deve ser informado");
                    }
                    else if (!eRenovacao && entity.Segmento == null && entity.Origem != 3)
                    {
                        ThrowValidationError("Segmento", "Segmento deve ser informado");
                    }
                    else if (entity.Segurado.VinculoBNB == null)
                    {
                        ThrowValidationError("Vínculo BNB", "Vínculo BNB deve ser informado");
                    }
                    else if (entity.AgenciaConta == null)
                    {
                        ThrowValidationError("Agencia Conta", "Agencia Conta deve ser informada");
                    }
                    if ((eRenovacao || eProspeccao) && (entity.DataFimVigencia == null))
                    {
                        ThrowValidationError("Data Fim Vigência", "Data Fim Vigência deve ser informado");
                    }

                }
            }
            else
            {
                if (eSinistro)
                {
                    if (entity.OperacaoDeFinanciamento != null)
                    {
                        ThrowValidationError("Operação de Financiamento",
                            "Para solicitações do tipo Sinistro, o campo Operação de Financiamento não deve ser preenchido");
                    }
                    else if (entity.Segmento != null)
                    {
                        ThrowValidationError("Segmento",
                            "Para solicitações do tipo Sinistro, o campo Segmento não deve ser preenchido");
                    }
                    else if (entity.Segurado.VinculoBNB != null)
                    {
                        ThrowValidationError("Vínculo BNB",
                            "Para solicitações do tipo Sinistro, o campo Vínculo BNB não deve ser preenchido");
                    }
                    else if (entity.DataFimVigencia != null)
                    {
                        ThrowValidationError("Data Fim Vigência",
                            "Para solicitações do tipo Sinistro, o campo Data Fim Vigência não deve ser preenchido");
                    }
                }
                else
                {
                    if (entity.OperacaoDeFinanciamento == null)
                    {
                        ThrowValidationError("Operação de Financiamento", "Operação de Financiamento deve ser informado");
                    }
                    else if (!eRenovacao && entity.Segmento == null && entity.Origem != 3)
                    {
                        ThrowValidationError("Segmento", "Segmento deve ser informado");
                    }
                    else if (entity.Segurado.VinculoBNB == null)
                    {
                        ThrowValidationError("Vínculo BNB", "Vínculo BNB deve ser informado");
                        if ((eRenovacao || eProspeccao) && (entity.DataFimVigencia == null))
                        {
                            ThrowValidationError("Data Fim Vigência", "Data Fim Vigência deve ser informado");
                        }

                    }
                }
            }
        }*/
    }
}
