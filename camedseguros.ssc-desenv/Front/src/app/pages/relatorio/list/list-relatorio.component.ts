import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import * as moment from "moment";

import { BaseComponent, AuthenticationService, User, AgenciaService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from "sweetalert2";
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  // //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs,
  Filters,
  GridService,
  BackendService,
} from "angular-slickgrid";

import { RelatorioService } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';
import { GridOdataService } from '@slickgrid-universal/odata';
import { Query } from 'apollo-angular';
import { type } from 'os';
import { UsuarioService } from "../../usuario";
import { TipoSeguroService } from "../../tiposdeseguro";
import { TipoDeProdutoService } from '../../texto-personalizado-parametro/service/tipoDeProduto.service';
import { AreaDeNegocioService } from '../../areadenegocio';
import { SituacaoService } from "../../situacoes";

import * as html2pdf from "html2pdf.js";
import { UsuarioGrupo } from "../../auditoria";
import { select } from "underscore";
import { Console } from "console";

const XLSX = require("xlsx");

@Component({
  templateUrl: "list-relatorio.component.html",
})
export class ListRelatorioComponent extends BaseComponent implements OnInit {
  auditoria$: any;
  itemSelecionado: any;
  itemSelecionadoDef: any[] = []
  itemsImprimir: any;
  loggedUser: User;
  isAdm: boolean = false;
  isGerente: boolean = false;
  isAtendente: boolean = false;
  isSolicitante: boolean = false;
  relatorioType: string = "0";
  atendentes$: any;
  super$: any;
  agencias$: any;
  tipoDeSeguro$: any;
  tipoDeProduto$: any;
  areaDeNegocio$: any;
  situacoes$: any;
  relatorio: any = {};
  idFinal: any;
  relatorioData: any;

  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[] = [];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = "";
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;
  odataUrl: string = null;

  @Output() item = new EventEmitter<any>();


  constructor(private service: RelatorioService,
    private userGrupoService: UsuarioGrupo,
    private tipodeDeSeguro: TipoSeguroService,
    private usuarioService: UsuarioService,
    private agenciaService: AgenciaService,
    private tipoDeProdutoService: TipoDeProdutoService,
    private areaDeNegocioService: AreaDeNegocioService,
    private situacaoService: SituacaoService,

    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    this.loggedUser = this.authenticationService.getLoggedUser();
    this.userGrupoService.get(`$select=usuario_Id,grupo_Id&$filter=usuario_Id eq ${this.loggedUser.id}`).subscribe((data) => {
      data.value.forEach(element => {
        switch(element.grupo_Id){
          case 1:
            this.isAtendente = true;
            $("#relatorioType option[value=2]").hide();
            $("#relatorioType option[value=4]").hide();
          break;
          case 2:
            this.isGerente = true;
          break;
          case 3:
            this.isAdm = true;
          break;
          case 4:
            this.isSolicitante = true;
            $("#relatorioType option[value=3]").hide();
            $("#relatorioType option[value=4]").hide();
            $("#relatorioType option[value=5]").hide();
            $("#relatorioType option[value=6]").hide();
            $("#relatorioType option[value=7]").hide();
            $("#relatorioType option[value=8]").hide();
            $("#relatorioType option[value=9]").hide();
            $("#relatorioType option[value=10]").hide();
            $("#relatorioType option[value=11]").hide();
          break;
        }
      });
      console.log(this.isGerente,this.isAdm)
    });

    let relatorio = (<HTMLInputElement>document.getElementById("relatorioType"));
    relatorio.addEventListener("change", () => {
      switch(relatorio.value){
        case "0":
          this.relatorioType = "0";
          break;
        case "1":
          this.relatorioType = "1";
          break;
        case "2":
          this.relatorioType = "2";
          break;
        case "3":
          this.relatorioType = "3";
          break;
        case "4":
          this.relatorioType = "4";
          break;
        case "5":
          this.relatorioType = "5";
          break;
        case "6":
          this.relatorioType = "6";
          break;
        case "7":
          this.relatorioType = "7";
          break;
        case "8":
          this.relatorioType = "8";
          break;
        case "9":
          this.relatorioType = "9";
          break;
        case "10":
          this.relatorioType = "10";
          break;
        case "11":
          this.relatorioType = "11";
          break;
        default:
          console.log("Relatório não definido.");
      }
    });

    this.atendentes$ = this.usuarioService.get("$select=id,nome");
    this.super$ = this.agenciaService.get("$select=superId,super");
    this.agencias$ = this.agenciaService.get("$select=id,codigo,nome");
    this.tipoDeSeguro$ = this.tipodeDeSeguro.get("$select=id,nome");
    this.tipoDeProduto$ = this.tipoDeProdutoService.get("$select=id,nome");
    this.areaDeNegocio$ = this.areaDeNegocioService.get("$select=id,nome");
    this.situacoes$ = this.situacaoService.get("$select=id,nome");
  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    //
  }

  excel(){
    const wb = XLSX.utils.book_new();
    let relatorio = (<HTMLInputElement>document.getElementById("relatorioType"));
    
    this.relatorio = {
      tipoRelatorio: relatorio.value == "0" ? "Relatório de Acompanhamentos" : relatorio.value == "1" ? "Monitoria" : relatorio.value == "2" ? "Canceladas" : relatorio.value == "3" ? "Avaliação de Atendimento" : relatorio.value == "4" ? "Capa da Proposta de Solicitação" : relatorio.value == "5" ? "Relatório de Conversão" : relatorio.value == "6" ? "Relatório de Prevenção de Recusas" : relatorio.value == "7" ? "Relatório de Pendência de Documentação (sinistro)" : relatorio.value == "8" ? "Contabilização Solicitação por Agência" : relatorio.value == "9" ? "Agendamentos de Ligação" : relatorio.value == "10" ? "Relatório de Inbox" : relatorio.value == "11" ? "Relatório de Checkin" : "Relatório não identificado.",
      solicitacao: (<HTMLInputElement>document.getElementById("solicitacao")) == null ? "" : (<HTMLInputElement>document.getElementById("solicitacao")).value,
      segurado: (<HTMLInputElement>document.getElementById("segurado")) == null ? "" : (<HTMLInputElement>document.getElementById("segurado")).value,
      superintendencia: (<HTMLInputElement>document.getElementById("superintendencia")) == null ? "" : (<HTMLInputElement>document.getElementById("superintendencia")).value,
      operador: (<HTMLInputElement>document.getElementById("operador")) == null ? "" : (<HTMLInputElement>document.getElementById("operador")).value,
      superConta: (<HTMLInputElement>document.getElementById("superConta")) == null ? "" : (<HTMLInputElement>document.getElementById("superConta")).value,
      atendente: (<HTMLInputElement>document.getElementById("atendente")) == null ? "" : (<HTMLInputElement>document.getElementById("atendente")).value,
      cnpj: (<HTMLInputElement>document.getElementById("cnpj")) == null ? "" : (<HTMLInputElement>document.getElementById("cnpj")).value,
      dataInicial: (<HTMLInputElement>document.getElementById("dataInicial")) == null ? "" : (<HTMLInputElement>document.getElementById("dataInicial")).value,
      dataFinal: (<HTMLInputElement>document.getElementById("dataFinal")) == null ? "" : (<HTMLInputElement>document.getElementById("dataFinal")).value,
      dataInicialAC: (<HTMLInputElement>document.getElementById("dataInicialAC")) == null ? "" : (<HTMLInputElement>document.getElementById("dataInicialAC")).value,
      dataFinalAC: (<HTMLInputElement>document.getElementById("dataFinalAC")) == null ? "" : (<HTMLInputElement>document.getElementById("dataFinalAC")).value,
      dataFechamentoI: (<HTMLInputElement>document.getElementById("dataFechamentoI")) == null ? "" : (<HTMLInputElement>document.getElementById("dataFechamentoI")).value,
      dataFechamentoF: (<HTMLInputElement>document.getElementById("dataFechamentoF")) == null ? "" : (<HTMLInputElement>document.getElementById("dataFechamentoF")).value,
      areaNegocio: (<HTMLInputElement>document.getElementById("areaNegocio")) == null ? "" : (<HTMLInputElement>document.getElementById("areaNegocio")).value,
      tipoSeguro : (<HTMLInputElement>document.getElementById("tipoSeguro")) == null ? "" : (<HTMLInputElement>document.getElementById("tipoSeguro")).value,
      segmento: (<HTMLInputElement>document.getElementById("segmento")) == null ? "" : (<HTMLInputElement>document.getElementById("segmento")).value,
      ramoSeguro: (<HTMLInputElement>document.getElementById("ramoSeguro")) == null? "" : (<HTMLInputElement>document.getElementById("ramoSeguro")).value,
      status: (<HTMLInputElement>document.getElementById("status")) == null ? "" : (<HTMLInputElement>document.getElementById("status")).value,
      emProcesso: (<HTMLInputElement>document.getElementById("gridCheck")) == null ? "" : (<HTMLInputElement>document.getElementById("gridCheck")).checked == true ? "Sim" : "Não",
      vistoria: (<HTMLInputElement>document.getElementById("vistoria")) == null ? "" : (<HTMLInputElement>document.getElementById("vistoria")).value,
      situacao: document.getElementById("situacaoInput") == null ? "" : document.getElementById("situacaoInput").innerHTML,
      agencia: document.getElementById("agenciaInput") == null ? "" : document.getElementById("agenciaInput").innerHTML,
      agenciaConta: document.getElementById("agenciaContaInput") == null ? "" : document.getElementById("agenciaContaInput").innerHTML,
      usuarioId: this.loggedUser.id
    };

    this.service.getRelatorio(this.relatorio).subscribe(response => {
      this.relatorioData = JSON.parse(response);
      let dados = [];

      if (this.relatorioType == '0') {
        dados = [
          ['N* Solicitação:', 'Data Ingresso:', 'Hora Ingresso:', 'D/ Situação Atual:', 'H/ Situação Atual:', 'Agência:',
            'Superintendência:', 'Solicitante:', 'Área Negócio:', 'Tipo de Seguro:', 'Segmento:', 'Ramo de Seguro:',
            'Nome Segurado:', 'CNPJ/CPF/CEI:', 'Vinculo c/ BNB:', 'Operação Financiamento:', 'Data F/ Vigência:', 'Tipo Cancelamento:',
            'Dias p/ Renovação:', 'Atendente:', 'Operador:', 'Situação:', 'Acomp Data:', 'Acom Situação:',
            'Motivo da Recusa:', 'OBS:', 'Acom Atendente:', 'Grupo:', 'ACOMPSLADEF:', 'ACOMPSLAEFET:',
            'ACOMPSLAACUM:', 'Status Usuário:', 'Orçamento Prévio:', 'Projeto Crossup:', 'Mercado:', 'Agência Conta:',
            'Super Conta:', 'Seguradora:', 'Estudo Origem:', 'ACOMPATENDENTEREAL:']
        ];
        
        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATADEINGRESSO'], ele['HORAINGRESSO'], ele['DATASITUACAOATUAL'], ele['HORASITUACAOATUAL'], ele['AGENCIA'],
            ele['SUPERINTENDENCIA'], ele['SOLICITANTE'], ele['AREANEGOCIO'], ele['TIPODESEGURO'], ele['SEGMENTO'], ele['RAMO'],
            ele['NOMESEGURADO'], ele['CPFCNPJ'], ele['VINCULOCOMBNB'], ele['OPERACAODEFINANCIAMENTO'], ele['DATAFIMVIGENCIA'], ele['TIPOCANCELAMENTO'],
            ele['DIASPARARENOVACAO'], ele['ATENDENTE'], ele['OPERADOR'], ele['SITUACAO'], ele['ACOMPDATAFORMATADA'], ele['ACOMSITUACAO'],
            ele['MOTIVODARECUSA'], ele['OBSERVACAO'], ele['ACOMATENDENTE'], ele['GRUPO'], ele['ACOMPSLADEF'], ele['ACOMPSLAEFET'],
            ele['ACOMPSLAACUM'], ele['STATUSUSUARIO'], ele['OrcamentoPrevio'], ele['PROJETOCROSSUP'], ele['MERCADO'], ele['AGENCIACONTA'],
            ele['SUPERCONTA'], ele['SEGURADORA'], ele['ESTUDO_ORIGEM'], ele['ACOMPATENDENTEREAL']
          ];
          
          dados.push(newLine);
        });
      } 
      
      else if (this.relatorioType == '1') {
        dados = [
          ['N* Solicitação:', 'Data Ingresso:', 'Hora Ingresso:', 'D/ Situação Atual:', 'H/ Situação Atual:', 'Agência:',
            'Superintendência:', 'Solicitante:', 'Área Negócio:', 'Tipo de Seguro:', 'Segmento:', 'Ramo de Seguro:',
            'Nome Segurado:', 'CNPJ/CPF/CEI:', 'Vinculo c/ BNB:', 'Operação Financiamento:', 'Data F/ Vigência:', 'Tipo Cancelamento:',
            'Dias p/ Renovação:', 'Atendente:', 'Operador:', 'Situação:', 'Status Usuário:', 'Orçamento Prévio:',
            'Projeto Crossup:', 'Mercado', 'Perc Anterior:', 'VLRPREMIOLIQANT:', 'Perc Atual:', 'VLPREMIOLIQATUAL:',
            'Perc Proposta:', 'VLPREMIOLIQPROPOSTA', 'NUMNOVO', 'Agência Conta:', 'Super Conta:', 'Seguradora:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATADEINGRESSO'], ele['HORAINGRESSO'], ele['DATASITUACAOATUAL'], ele['HORASITUACAOATUAL'], ele['AGENCIA'],
            ele['SUPERINTENDENCIA'], ele['SOLICITANTE'], ele['AREANEGOCIO'], ele['TIPODESEGURO'], ele['SEGMENTO'], ele['RAMO'],
            ele['NOMESEGURADO'], ele['CPFCNPJ'], ele['VINCULOCOMBNB'], ele['OPERACAODEFINANCIAMENTO'], ele['DATAFIMVIGENCIA'], ele['TIPOCANCELAMENTO'],
            ele['DIASPARARENOVACAO'], ele['ATENDENTE'], ele['OPERADOR'], ele['SITUACAO'], ele['STATUSUSUARIO'], ele['OrcamentoPrevio'],
            ele['PROJETOCROSSUP'], ele['MERCADO'], ele['PERC_ANTERIOR'], ele['VLRPREMIOLIQANT'], ele['PERC_ATUAL'], ele['VLPREMIOLIQATUAL'],
            ele['PERC_PROPOSTA'], ele['VLPREMIOLIQPROPOSTA'], ele['NUMNOVO'], ele['AGENCIACONTA'], ele['SUPERCONTA'], ele['SEGURADORA'],
          ];

          dados.push(newLine);
        });
      }
      
      else if (this.relatorioType == '2') {
        dados = [
          ['N* Solicitação', 'Observação']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [ele['NUMERO'], ele['OBSERVACAO']];

          dados.push(newLine);
        });
      } 
      
      else if (this.relatorioType == '3') {
        dados = [
          ['Data Avaliação:', 'N* Solicitação:', 'Tipo de Seguro:', 'Ramo de Seguro:', 'Nome Segurado:', 'Agência:',
            'Superintendência:', 'Área de Negócio:', 'Atendente:', 'Operador:', 'Nota:', 'Observação:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['DATAAVALIACAO'], ele['NUMERO'], ele['TIPODESEGURO'], ele['RAMO'], ele['SEGURADO'], ele['AGENCIa'],
            ele['SUPERINTENDENCIA'], ele['AREADENEGOCIO'], ele['ATENDENTE'], ele['OPERADOR'], ele['NOTA'], ele['OBSERVACAO']
          ];

          dados.push(newLine);
        });
      }

      else if (this.relatorioType == '4') {
        dados = [
          ['N* Solicitação:', 'Data de Ingresso:', 'Hora Ingresso:', 'Data Situação Atual:', 'Hora Situação Atual:', 'Agência:',
            'Superintendência:', 'Solicitante:', 'Área Negócio:', 'Tipo de Seguro:', 'Segmento:', 'Ramo de Seguro:',
            'Nome Segurado:', 'CNPJ/CPF/CEI:', 'Vinculo c/ BNB:', 'Operação Financiamento:', 'Data F/ Vigência:', 'Dias p/ Renovação:',
            'Tipo Cancelamento:', 'Atendente:', 'Operador:', 'Situação:', 'Status Usuário:', 'Orçamento Préveio:',
            'Projeto Crossup:', 'Mercado:', 'Nome Seguradora:', 'Ramo GS:', 'Tipo Seguro GS:', 'N* Apólice Ant:',
            'Perc Comissão:', 'Perc Agênciamento:', 'Valor da Importância Segurado:', 'Forma Pgto 1A:', 'Forma Pgto Demais:', 'Sede Envia Doc Físico:',
            'N* Vistoria:', 'Cadastrado GS:', 'CD Estudo:', 'VIP:', 'RECHACO:', 'Tipo Comissão RV:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATADEINGRESSO'], ele['HORAINGRESSO'], ele['DATASITUACAOATUAL'], ele['HORASITUACAOATUAL'], ele['AGENCIA'],
            ele['SUPERINTENDENCIA'], ele['SOLICITANTE'], ele['AREANEGOCIO'], ele['TIPODESEGURO'], ele['SEGMENTO'], ele['RAMO'],
            ele['NOMESEGURADO'], ele['CPFCNPJ'], ele['VINCULOCOMBNB'], ele['OPERACAODEFINANCIAMENTO'], ele['DATAFIMVIGENCIA'], ele['DIASPARARENOVACAO'],
            ele['TIPOCANCELAMENTO'], ele['ATENDENTE'], ele['OPERADOR'], ele['SITUACAO'], ele['STATUSUSUARIO'], ele['OrcamentoPrevio'],
            ele['PROJETOCROSSUP'], ele['MERCADO'], ele['NOME_SEGURADORA'], ele['RAMO_GS'], ele['TIPO_DE_SEGURO_GS'], ele['NUMERO_APOLICE_ANT'],
            ele['PERC_COMISSAO'], ele['PERC_AGENCIAMENTO'], ele['VALOR_DA_IMPORTANCIA_SEGURADO'], ele['FORMA_PGTO_1A'], ele['FORMA_PGTO_DEMAIS'], ele['SEDE_ENVIA_DOC_FISICO'],
            ele['NUMERO_VISTORIA'], ele['CADASTRADOGS'], ele['CD_ESTUDO'], ele['VIP'], ele['RECHACO'], ele['TIPOCOMISSAORV']
          ];

          dados.push(newLine);
        });
      }
      
      else if (this.relatorioType == '5') {
        dados = [
          ['N* Solicitação:', 'Data Ingresso:', 'Hora Ingresso:', 'D/ Situação Atual:', 'H/ Situação Atual:', 'Agência:',
            'Superintendência:', 'Solicitante:', 'Área Negócio:', 'Tipo de Seguro:', 'Segmento:', 'Ramo de Seguro:',
            'Nome Segurado:', 'CNPJ/CPF/CEI:', 'Vinculo c/ BNB:', 'Atendente:', 'Situação', 'Operação Financiamento:',
            'Data F/ Vigência:', 'Tipo Cancelamento:', 'Dias p/ Renovação:', 'Status Usuário:', 'Tempo Decorrido:', 'Data Cotação:',
            'Hora Contação:', 'Situação Cotação:', 'Data Fechamento:', 'Situação Fechamento:', 'MESANOINGRESSO:', 'MESANOCOTACAO:',
            'Tempo Total:', 'MESANOFECHAMENTO:', 'VLR_PL_ANTERIOR:', 'Perc c/ Anterior', 'VLR_PL_COTACAO:', 'Perc c/ Cotação:',
            'SGD_COTACAO:', 'VLR_PL_FINAL:', 'Perc c/ Final:', 'PERC_CO_CORRETAGEM:', 'SGD_FINAL:', 'ESTUDO/PROPOSTA:',
            'Venda Compartilhada:', 'N* Proposta Seguradora:', 'VL Importância Seguradora:', 'Mercado:', 'Estudo Origem:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATADEINGRESSO'], ele['HORAINGRESSO'], ele['DATASITUACAOATUAL'], ele['HORASITUACAOATUAL'], ele['AGENCIA'],
            ele['SUPERINTENDENCIA'], ele['SOLICITANTE'], ele['AREANEGOCIO'], ele['TIPODESEGURO'], ele['SEGMENTO'], ele['RAMO'],
            ele['NOMESEGURADO'], ele['CPFCNPJ'], ele['VINCULOCOMBNB'], ele['ATENDENTE'], ele['SITUACAO'], ele['OPERACAODEFINANCIAMENTO'],
            ele['DATAFIMVIGENCIA'], ele['TIPOCANCELAMENTO'], ele['DIASPARARENOVACAO'], ele['STATUSUSUARIO'], ele['TEMPO_DECORRIDO'], ele['DATACOTACAO'],
            ele['HORACOTACAO'], ele['SITUACAOCOTACAO'], ele['DATAFECHAMENTO'], ele['SITUACAOFECHAMENTO'], ele['MESANOINGRESSO'], ele['MESANOCOTACAO'],
            ele['TEMPOTOTAL'], ele['MESANOFECHAMENTO'], ele['VLR_PL_ANTERIOR'], ele['PERC_COM_ANTERIOR'], ele['VLR_PL_COTACAO'], ele['PERC_COM_COTACAO'],
            ele['SGD_COTACAO'], ele['VLR_PL_FINAL'], ele['PERC_COM_FINAL'], ele['PERC_CO_CORRETAGEM'], ele['SGD_FINAL'], ele['ESTUDO/PROPOSTA'],
            ele['VENDA_COMPARTILHADA'], ele['NUMERO_PROPOSTA_SEGURADORA'], ele['VL_IMPORTANCIA_SEGURADA'], ele['MERCADO'], ele['ESTUDO_ORIGEM'],
          ];

          dados.push(newLine);
        });
      }

      else if (this.relatorioType == '7') {
        dados = [
          ['N* Solicitação:', 'Data de Ingresso:', 'Nome Segurado:', 'Agência:', 'Agência Conta:', 'CNPJ/CPF/CEI:', 'Ramo de Seguro:', 'Nome do Documento:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATADEINGRESSO'], ele['NOMESEGURADO'], ele['AGENCIA'], ele['AGENCIACONTA'], ele['CPFCNPJ'], ele['RAMO'], ele['NOMEDOCUMENTO']
          ];

          dados.push(newLine);
        });
      }

      else if (this.relatorioType == '8') {
        dados = [
          ['Nome:', 'Total:', 'Canceladas Recusadas:', 'Transmitidas sem Retorno:', 'Devolvidas Assinadas Total:', 'Devolvidas Assinadas Automático:',
            'Devolvidas Assinadas Manual:', 'Em Processo:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['Nome'], ele['Total'], ele['CanceladasRecusadas'], ele['TransmitidasSemRetorno'], ele['DevolvidasAssinadasTotal'], ele['DevolvidasAssinadasAutomático'],
            ele['DevolvidasAssinadasManual'], ele['EmProcesso'],
          ];

          dados.push(newLine);
        });
      }

      else if (this.relatorioType == '9') {
        dados = [
          ['N* Solicitação:', 'Data Agendamento:', 'Hora Agendamento:', 'Objetivo Ligação:', 'Data Ligação:', 'Hora Ligação:',
            'Retorno Ligação:', 'Agência:', 'Superintendência:', 'Solicitante:', 'Área Negócio:', 'Tipo de Seguro:',
            'Ramo de Seguro:', 'Nome Segurado:', 'CNPJ/CPF/CEI:', 'Atendente:', 'Situação:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['NUMERO'], ele['DATAAGENDAMENTO'], ele['HORAAGENDAMENTO'], ele['OBJETIVOLIGACAO'], ele['DATALIGACAO'], ele['HORALIGACAO'],
            ele['RETORNOLIGACAO'], ele['AGENCIA'], ele['SUPERINTENDENCIA'], ele['SOLICITANTE'], ele['AREANEGOCIO'], ele['TIPODESEGURO'],
            ele['RAMO'], ele['NOMESEGURADO'], ele['CPFCNPJ'], ele['ATENDENTE'], ele['SITUACAO'],
          ];

          dados.push(newLine);
        });
      }

      else if (this.relatorioType == '10') {
        dados = [
          ['Remetente:', 'Destinatário:', 'Assunto:', 'Texto:', 'Lida:', 'Data de Criação:']
        ];

        console.log(this.relatorioData);

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['REMETENTE'], ele['DESTINATARIO'], ele['ASSUNTO'], ele['TEXTO'], ele['LIDA'], ele['DATACRIACAO']
          ];

          dados.push(newLine);
        });
      }
      
      else if (this.relatorioType == '11') {
        dados = [
          ['Data:', 'Hora:', 'Longitude:', 'Latitude:', 'Localidade:', 'Endereço', 'Número:', 'Atendente:']
        ];

        this.relatorioData.forEach(ele => {
          let newLine = [
            ele['DATA'], ele['HORA'], ele['LONGITUDE'], ele['LATITUDE'], ele['LOCALIDADE'], ele['ENDERECO'], ele['NUMERO'], ele['ATENDENTE']
          ];

          dados.push(newLine);
        });
      }

      wb.Props = {
        Title: this.renameTitle(relatorio.value),
        Subject: '',
        Author: 'CAMED',
        CreatedDate: new Date(),
      };
      wb.SheetNames.push(wb.Props.Title);

      const ws = XLSX.utils.aoa_to_sheet(dados);

      wb.Sheets[wb.Props.Title] = ws;

      XLSX.writeFile(wb, wb.Props.Title + '.xlsx', { bookType: 'xlsx', type: 'bynary' });
    });
  }

  renameTitle(tipoRelatorio) {
    if (tipoRelatorio != '7' && tipoRelatorio != '8') return this.relatorio.tipoRelatorio
    else if (tipoRelatorio == '7') return "Relatório Sinistro"
    else if (tipoRelatorio == '8') return "Cont Agência"
  }

  imprimir() {
    let relatorio = (<HTMLInputElement>document.getElementById("relatorioType"));

    this.relatorio = {
      tipoRelatorio: relatorio.value == "0" ? "Relatório de Acompanhamentos" : relatorio.value == "1" ? "Monitoria" : relatorio.value == "2" ? "Canceladas" : relatorio.value == "3" ? "Avaliação de Atendimento" : relatorio.value == "4" ? "Capa da Proposta de Solicitação" : relatorio.value == "5" ? "Relatório de Conversão" : relatorio.value == "6" ? "Relatório de Prevenção de Recusas" : relatorio.value == "7" ? "Relatório de Pendência de Documentação (sinistro)" : relatorio.value == "8" ? "Contabilização Solicitação por Agência" : relatorio.value == "9" ? "Agendamentos de Ligação" : relatorio.value == "10" ? "Relatório de Inbox" : relatorio.value == "11" ? "Relatório de Checkin" : "Relatório não identificado.",
      solicitacao: (<HTMLInputElement>document.getElementById("solicitacao")) == null ? null : (<HTMLInputElement>document.getElementById("solicitacao")).value,
      segurado: (<HTMLInputElement>document.getElementById("segurado")) == null ? null : (<HTMLInputElement>document.getElementById("segurado")).value,
      superintendencia: (<HTMLInputElement>document.getElementById("superintendencia")) == null ? null : (<HTMLInputElement>document.getElementById("superintendencia")).value,
      operador: (<HTMLInputElement>document.getElementById("operador")) == null ? null : (<HTMLInputElement>document.getElementById("operador")).value,
      superConta: (<HTMLInputElement>document.getElementById("superConta")) == null ? null : (<HTMLInputElement>document.getElementById("superConta")).value,
      atendente: (<HTMLInputElement>document.getElementById("atendente")) == null ? null : (<HTMLInputElement>document.getElementById("atendente")).value,
      cnpj: (<HTMLInputElement>document.getElementById("cnpj")) == null ? null : (<HTMLInputElement>document.getElementById("cnpj")).value,
      dataInicial: (<HTMLInputElement>document.getElementById("dataInicial")) == null ? null : (<HTMLInputElement>document.getElementById("dataInicial")).value,
      dataFinal: (<HTMLInputElement>document.getElementById("dataFinal")) == null ? null : (<HTMLInputElement>document.getElementById("dataFinal")).value,
      dataInicialAC: (<HTMLInputElement>document.getElementById("dataInicialAC")) == null ? null : (<HTMLInputElement>document.getElementById("dataInicialAC")).value,
      dataFinalAC: (<HTMLInputElement>document.getElementById("dataFinalAC")) == null ? null : (<HTMLInputElement>document.getElementById("dataFinalAC")).value,
      dataFechamentoI: (<HTMLInputElement>document.getElementById("dataFechamentoI")) == null ? null : (<HTMLInputElement>document.getElementById("dataFechamentoI")).value,
      dataFechamentoF: (<HTMLInputElement>document.getElementById("dataFechamentoF")) == null ? null : (<HTMLInputElement>document.getElementById("dataFechamentoF")).value,
      areaNegocio: (<HTMLInputElement>document.getElementById("areaNegocio")) == null ? null : (<HTMLInputElement>document.getElementById("areaNegocio")).value,
      tipoSeguro : (<HTMLInputElement>document.getElementById("tipoSeguro")) == null ? null : (<HTMLInputElement>document.getElementById("tipoSeguro")).value,
      segmento: (<HTMLInputElement>document.getElementById("segmento")) == null ? null : (<HTMLInputElement>document.getElementById("segmento")).value,
      ramoSeguro: (<HTMLInputElement>document.getElementById("ramoSeguro")) == null? null : (<HTMLInputElement>document.getElementById("ramoSeguro")).value,
      status: (<HTMLInputElement>document.getElementById("status")) == null ? null : (<HTMLInputElement>document.getElementById("status")).value,
      emProcesso: (<HTMLInputElement>document.getElementById("gridCheck")) == null ? null : (<HTMLInputElement>document.getElementById("gridCheck")).checked == true ? "Sim" : "Não",
      vistoria: (<HTMLInputElement>document.getElementById("vistoria")) == null ? null : (<HTMLInputElement>document.getElementById("vistoria")).value,
      situacao: document.getElementById("situacaoInput") == null ? null : document.getElementById("situacaoInput").innerHTML,
      agencia: document.getElementById("agenciaInput") == null ? null : document.getElementById("agenciaInput").innerHTML,
      agenciaConta: document.getElementById("agenciaContaInput") == null ? null : document.getElementById("agenciaContaInput").innerHTML,
    };
    setTimeout(() => {
      var printContents = document.getElementById('tudo');
      var options = {
        filename: 'Relatorio.pdf',
        image: { type: 'jpeg'},
        html2canvas: {},
        jsPDF: { orientation: 'landscape' }
      }

      html2pdf().from(printContents).set(options).save()
    }, 1)

  }

  funcaoBotao(){
    let formato = (<HTMLInputElement>document.getElementById("formato")).value;

    if(formato == "PDF") {
      this.imprimir();
    } else {
      this.excel();
    }
  }
}

// if (window) {
    //   if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
    //     var popup = window.open('', '_blank',
    //       'width=600,height=600,scrollbars=no,menubar=no,toolbar=no,'
    //       + 'location=no,status=no,titlebar=no');

    //     popup.window.focus();
    //     popup.document.write('<!DOCTYPE html><html><head>'
    //       + '</head><body onload="window.print()"><div class="reward-body">'
    //       + printContents + '</div></html>');
    //     popup.onbeforeunload = function (event) {
    //       popup.close();
    //       return '.\n';
    //     };
    //     popup.onabort = function (event) {
    //       popup.document.close();
    //       popup.close();
    //     }
    //   } else {
    //     var popup = window.open('', '_blank', 'width=800,height=600');
    //     popup.document.open();
    //     popup.document.write('<html><head>' +
    //       +'</head><body onload="window.print()">' + printContents + '</html>');
    //     popup.document.close();
    //   }

    //   popup.document.close();
    // }
      // return true;
