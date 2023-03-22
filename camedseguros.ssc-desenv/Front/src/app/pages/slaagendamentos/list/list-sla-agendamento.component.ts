import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Observable } from "rxjs";

import { BaseComponent, AuthenticationService, Grupo } from "src/app/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import * as moment from 'moment';
import flatpickr from "flatpickr";


import Swal from "sweetalert2";
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs,
} from "angular-slickgrid";
import { GridOdataService } from "@slickgrid-universal/odata";

import { SLAAgendamentoService } from "../service";
import { localePtBR } from "src/locale/slickgrid.pt";

@Component({
  
  templateUrl: "list-sla-agendamento.component.html",
})
export class ListAgendamentoComponent extends BaseComponent implements OnInit {
  display = false;
  grupos$: Observable<Grupo>;
  grupoSelecionado: any;
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
  consultando = false;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;

  dataFinal:any;
  dataInicial:any;
  situacao:any;

  @Output() grupo = new EventEmitter<any>();

  constructor(
    private grupoService: SLAAgendamentoService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }


  ngOnInit() {
    document.getElementById("alerta").style.display="none";
    this.columnDefinitions = [
      // {
      //   id: "delete",
      //   field: "",
      //   toolTip: "Excluir",
      //   excludeFromExport: true,
      //   excludeFromHeaderMenu: true,
      //   excludeFromGridMenu: true,
      //   excludeFromColumnPicker: true,
      //   formatter: Formatters.deleteIcon,
      //   minWidth: 30,
      //   maxWidth: 30,

      //   onCellClick: (e: Event, args: OnEventArgs) => {
      //     this.remover(args);
      //   },
      // },
      {
        id: 'dataEHoraInicial', 
        name: '',
        field: 'dataEHoraInicial', 
        outputType: FieldType.dateTimeShortEuro,
        formatter: Formatters.dateTimeShortEuro,
      },
      {
        id: 'dataEHoraFinal', 
        name: '',
        field: 'dataEHoraFinal', 
        outputType: FieldType.dateTimeShortEuro,
        formatter: Formatters.dateTimeShortEuro,
      },
      {
        id: "situacao",
        name: "",
        field: "situacao",
      },
    ];

    this.gridOptions = {
      alwaysShowVerticalScroll: false,
      autoResize: {
        maxWidth: 1200,
      },
      enableEmptyDataWarningMessage: true,
      emptyDataWarning: {
        message: "Nenhum dado encontrado",
      },
      enableFiltering: false,
      enablePagination: true,
      enableAutoResize: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5,10,15,20, 25, 30, 40, 50, 75, 100],
        pageSize: 20,
        totalItems: 0,
      },
      backendServiceApi: {
        service: new GridOdataService(),
        options: {
          enableCount: this.isCountEnabled,
          version: 4,
        },
        preProcess: () => this.displaySpinner(true),
        process: (query) => this.getData(query),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.getCallback(response);
          this.displaySpinner(false);
          this.getCallback(response);
        },
      },
    };
  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  getCallback(response) {
    this.gridOptions.pagination.totalItems = response["@odata.count"];

    if (this.metrics) {
      this.metrics.totalItemCount = response["@odata.count"];
    }

    this.gridOptions = Object.assign({}, this.gridOptions);
    this.dataset = response.value;
    this.hasInitialLoading = false;
  }

  private displaySpinner(isProcessing) {
    this.loadingDataGrid = isProcessing;
  }

  getData(query) {
    this.odataQuery = query;

    let dtI = new Date(this.dataInicial);
    let dtF = new Date(this.dataFinal);
    let auxI = this.dataInicial
    let auxF = this.dataFinal

    if($('#DI').val() == "" && $('#DF').val() == ""){
      return this.grupoService.getAll(`${query}&$orderby=dataEHoraInicial%20desc`);
    }

    if(this.dataInicial && !this.dataFinal && this.situacao){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = "2222-11-01T10:03";
      let dtF = new Date(this.dataFinal)
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = auxI;
      this.dataFinal = null;
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataInicial} and dataEHoraInicial le ${novaDataFinal})and(situacao eq ${this.situacao})&$orderby=dataEHoraInicial%20desc`);
    }

    if(!this.dataInicial && this.dataFinal && this.situacao){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = "2222-11-01T10:03";
      let dtI = new Date(this.dataInicial)
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = null;
      this.dataFinal = auxF;
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataFinal} and dataEHoraInicial le ${novaDataInicial})and(situacao eq ${this.situacao})&$orderby=dataEHoraInicial%20desc`);
    }
    
    if(this.dataInicial && this.dataFinal && this.situacao){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = auxI;
      this.dataFinal = auxF
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataInicial} and dataEHoraInicial le ${novaDataFinal})and(situacao eq ${this.situacao})&$orderby=dataEHoraInicial%20desc`);
    }

    if(this.dataInicial && this.dataFinal){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = auxI;
      this.dataFinal = auxF
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataInicial} and dataEHoraInicial le ${novaDataFinal})&$orderby=dataEHoraInicial%20desc`);
    }

    if(this.dataInicial && !this.dataFinal){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = "2222-11-01T10:03";
      let dtF = new Date(this.dataFinal)
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = auxI;
      this.dataFinal = null;
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataInicial} and dataEHoraInicial le ${novaDataFinal})&$orderby=dataEHoraInicial%20desc`);
    }

    if(!this.dataInicial && this.dataFinal){
      $('#alerta').css("display", "none")
      //Converte pro tipo reconhecido do banco
      this.dataInicial = "2222-11-01T10:03";
      let dtI = new Date(this.dataInicial)
      this.dataInicial = new Date(dtI.getTime() - (dtI.getTimezoneOffset() * 60000)).toISOString();
      this.dataInicial = this.dataInicial.replace("000Z","03:00");
      this.dataInicial = this.dataInicial.replace(".", "-");
      this.dataInicial = this.dataInicial.replace("00-", "00.000-");

      //Converte pro tipo reconhecido do banco
      this.dataFinal = new Date(dtF.getTime() - (dtF.getTimezoneOffset() * 60000)).toISOString();
      this.dataFinal = this.dataFinal.replace("000Z","03:00");
      this.dataFinal = this.dataFinal.replace(".", "-");
      this.dataFinal = this.dataFinal.replace("00-", "00.000-");

      //Guarda os valores numa variavel auxiliar
      let novaDataInicial = this.dataInicial;
      let novaDataFinal = this.dataFinal;

      //Altera pro valor preenchido do input. Isso faz não perder o valor ao dar refresh na página (As variávais abaixo são [(ngModel)] )
      this.dataInicial = null;
      this.dataFinal = auxF;
      
      return this.grupoService.getAll(`${query}&$filter=(dataEHoraInicial ge ${novaDataFinal} and dataEHoraInicial le ${novaDataInicial})&$orderby=dataEHoraInicial%20desc`);
    }
    
    return this.grupoService.getAll(`${query}&$orderby=dataEHoraInicial%20desc`);
    // 
  }

  reloadGrid() {
    if($('#DI').val() == ""){
      $('#alerta').css("display", "block")
      this.angularGrid.gridService.resetGrid();
    } else {
      this.angularGrid.gridService.resetGrid();
    }
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.grupoSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        "",
        `Grupo ${retorno.adicionar ? "adicionado" : "atualizado"}!`,
        "success"
      );
    }
  }

  openPanel() {
    this.router.navigate(["cadastrar"], { relativeTo: this.route });
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.grupoSelecionado = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: "Atenção!",
      text: `Deseja excluir o grupo: ${item.dataContext.nome}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.grupoService.deletar(item.dataContext.id).subscribe((response) => {
          if (response.successfully) {
            const metadata =
              this.angularGrid.gridService.getColumnFromEventArguments(item);
            this.angularGrid.gridService.deleteItemById(
              metadata.dataContext.id
            );
            Swal.fire("", response.message, "success");
          } else {
            Swal.fire("", response.message, "error");
          }
        });
      },
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`], { relativeTo: this.route });
  }
}
