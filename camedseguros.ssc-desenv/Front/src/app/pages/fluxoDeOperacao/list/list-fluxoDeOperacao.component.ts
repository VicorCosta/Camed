import { Observable } from 'rxjs';
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";

import Swal from "sweetalert2";

import { BaseComponent, AuthenticationService } from "src/app/core";
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

import { FluxoDeOperacaoService } from "../service";
import { SituacaoService } from "../../situacoes";
import { AcaoDeAcompanhamentoService } from "../../acaodeacompanhamento";
import { GrupoService } from "../../grupo";
import { localePtBR } from "src/locale/slickgrid.pt";
import { isNull } from "underscore";

@Component({
  selector: "app-list-fluxo-de-operacao",
  templateUrl: "./list-fluxoDeOperacao.component.html",
  styleUrls: ["./list-fluxoDeOperacao.component.css"],
})
export class ListFluxoDeOperacaoComponent
  extends BaseComponent
  implements OnInit
{
  display = false;
  itemSelecionado: any;

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

  @Output() vinculo = new EventEmitter<any>();

  constructor(
    private service: FluxoDeOperacaoService,
    private situacaoService: SituacaoService,
    private acaoService: AcaoDeAcompanhamentoService,
    private grupoService: GrupoService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    this.columnDefinitions = [
      {
        id: "edit",
        field: "",
        toolTip: "Editar",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.editIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
        },
      },
      {
        id: "delete",
        field: "",
        toolTip: "Excluir",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.deleteIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.remover(args.dataContext);
        },
      },
      {
        id: "ordemFluxo",
        name: "Ordem do Fluxo",
        field: "ordemFluxo",
        sortable: true,
        type: FieldType.number,
        filterable: true,
      },
      {
        id: "situacaoAtual",
        name: "Situação Atual",
        field: "situacaoAtual",
        sortable: true,
        queryFieldSorter: "situacaoAtual/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
        filterable: true,
        queryFieldFilter: "situacaoAtual/nome",
        filterSearchType: FieldType.number,
        filter: {
          collectionAsync: this.situacaoService.fetch(
            `$select=nome&$orderBy=nome`,
            false
          ),
          model: Filters.multipleSelect,
          customStructure: {
            label: "Nome",
            value: "Nome",
          },
        },
      },
      {
        id: "ordemFluxo2",
        name: "Ordem do Fluxo 2",
        field: "ordemFluxo2",
        sortable: true,
        type: FieldType.number,
        filterable: true,
      },
      {
        id: "acao",
        name: "Ação",
        field: "acao",
        sortable: true,
        queryFieldSorter: "acao/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
        filterable: true,
        queryFieldFilter: "acao/nome",
        filterSearchType: FieldType.number,
        filter: {
          collectionAsync: this.acaoService.fetch(
            `$select=nome&$orderBy=nome`,
            false
          ),
          model: Filters.multipleSelect,
          customStructure: {
            label: "Nome",
            value: "Nome",
          },
        },
      },
      {
        id: "proximaSituacao",
        name: "Próxima Situação",
        field: "proximaSituacao",
        sortable: true,
        queryFieldSorter: "proximaSituacao/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
        filterable: true,
        queryFieldFilter: "proximaSituacao/nome",
        filterSearchType: FieldType.number,
        filter: {
          collectionAsync: this.situacaoService.fetch(
            `$select=nome&$orderBy=nome`,
            false
          ),
          model: Filters.multipleSelect,
          customStructure: {
            label: "Nome",
            value: "Nome",
          },
        },
      },
      {
        id: "grupo",
        name: "Grupo",
        field: "grupo",
        sortable: true,
        queryFieldSorter: "grupo/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
        filterable: true,
        queryFieldFilter: "grupo/nome",
        filterSearchType: FieldType.number,
        filter: {
          collectionAsync: this.grupoService.fetch(
            `$select=nome&$orderBy=nome`,
            false
          ),
          model: Filters.multipleSelect,
          customStructure: {
            label: "Nome",
            value: "Nome",
          },
        },
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
      enableFiltering: true,
      enablePagination: true,
      enableAutoResize: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5, 10, 15, 20, 25, 30, 40, 50, 75, 100],
        pageSize: 5,
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

  getData(query){
    query = query.includes('$filter') ? query : '$filter=false';
    this.odataQuery = query;
    return this.service.getAll(`$expand=situacaoAtual,acao,proximaSituacao,grupo,parametrosSistema,parametroSistemaSMS&${query}`);
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.itemSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        "",
        `Fluxo de operação ${retorno.adicionar ? "adicionado" : "atualizado"}!`,
        "success"
      );
    }
  }

  openPanel() {
    this.router.navigate(["cadastrar"], { relativeTo: this.route });
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.itemSelecionado = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: "Atenção!",
      text: `Deseja excluir o fluxo de operação: ${item.id} ?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.service.deletar(item.id).subscribe(
          (response) => {
            if (response.successfully && item.enviaEmailAoSegurado === false) {
              Swal.fire("", "O fluxo de operação foi deletado.", "success");
              this.angularGrid.gridService.deleteItemById(item.id);
            } else {
              Swal.fire("", "Parametro não pode ser excluído.", "success");
            }
          },
          (erro) => {
            Swal.fire("", erro, "warning");
          }
        );
      },
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`], { relativeTo: this.route });
    // this.display = true;
  }
}
