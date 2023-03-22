import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from 'sweetalert2';

import { BaseComponent, AuthenticationService } from 'src/app/core';
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  // GridOdataService,

  Metrics,
  Formatters,
  OnEventArgs
} from 'angular-slickgrid';
import { GridOdataService } from '@slickgrid-universal/odata';

import { UsuarioService } from '../../usuario';
import { EmpresaService } from '../../empresas';
import { TipoSeguroService } from '../../tiposdeseguro';
import { GrupoAgenciaService } from '../../grupoagencias';
import { AgenciaService } from 'src/app/core';
import { AreaDeNegocioService } from '../../areadenegocio';
import { MapeamentoDeatendenteServices } from '../service';
import { filter } from 'underscore';


@Component({
  selector: 'app-list-mapeamento-de-atendente',
  templateUrl: './list-mapeamento-de-atendente.component.html',
  styleUrls: ['./list-mapeamento-de-atendente.component.css']
})

export class ListMapeamentoDeAtendenteComponent extends BaseComponent implements OnInit {
  display = false;
  itemSelecionado: any;

  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = '';
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;

  @Output() vinculo = new EventEmitter<any>();

  constructor(private service: MapeamentoDeatendenteServices,
    private usuarioServices: UsuarioService,
    private empresaService: EmpresaService,
    private agenciaService: AgenciaService,
    private grupoAgenciaService: GrupoAgenciaService,
    private tiposeguroService: TipoSeguroService,
    private areaDeNegocioService: AreaDeNegocioService,

    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {

    //#region Definição de colunas
    this.columnDefinitions = [
      {
        id: 'edit',
        field: '',
        toolTip: 'Editar',
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.editIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
        }
      },
      {
        id: 'delete',
        field: '',
        toolTip: 'Excluir',
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.deleteIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.remover(args);
        }
      },
      {
        id: 'tipoDeSeguro',
        name: 'Tipo de Seguro',
        field: 'tipoDeSeguro',
        sortable: true,
        queryFieldSorter: 'tipoDeSeguro/nome',
        formatter(row, cell, value, dataContext) {
              return value.nome;
            },
        filterable: true,
        queryFieldFilter: 'tipoDeSeguro/nome',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.tiposeguroService.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      
      {
        id: 'agencia',
        name: 'Agência',
        field: 'agencia',
        sortable: true,
        queryFieldSorter: 'agencia/nome',
        formatter(row, cell, value, dataContext) {
              return value.nome;
            },
        filterable: true,
        queryFieldFilter: 'agencia/nome',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.agenciaService.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      {
        id: 'grupoAgencia',
        name: 'Ramo de Negócio',
        field: 'grupoAgencia',
        sortable: true,
        queryFieldSorter: 'grupoAgencia/nome',
        formatter(row, cell, value, dataContext) {
              return value.nome;
            },
        filterable: true,
        queryFieldFilter: 'grupoAgencia/nome',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.grupoAgenciaService.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      {
        id: 'areaDeNegocio',
        name: 'Área de Negócio',
        field: 'areaDeNegocio',
        sortable: true,
        queryFieldSorter: 'areaDeNegocio/nome',
        formatter(row, cell, value, dataContext) {
              return value.nome;
            },
        filterable: true,
        queryFieldFilter: 'areaDeNegocio/nome',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.areaDeNegocioService.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      {
        id: 'atendente',
        name: 'Atendente',
        field: 'atendente',
        sortable: true,
        queryFieldSorter: 'atendente/nome',
        formatter(row, cell, value, dataContext) {
              return value.nome;
            },
        filterable: true,
        queryFieldFilter: 'atendente/nome',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.usuarioServices.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.autoComplete,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      {
        id: 'agenciasuper',
        name: 'Superintendência',
        field: 'agencia',
        sortable: true,
        queryFieldSorter: 'agencia/super',
        formatter(row, cell, value, dataContext) {
              return value.super;
            },
        filterable: true,
        queryFieldFilter: 'agencia/super',
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.agenciaService.fetch(`$select=super&$orderBy=super`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Super',
            value: 'Super'
          },
        }
      },
      
    ];
    //#endregion

    //#region Definção da Grid
    this.gridOptions = {
      enableAutoResize: true,

      alwaysShowVerticalScroll: false,
      autoResize: {
        maxWidth: 1200,
      },
      enableEmptyDataWarningMessage: true,
      emptyDataWarning: {
        message: 'Nenhum dado encontrado'
      },
      enableFiltering: true,
      enablePagination: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5, 10, 15, 20, 25, 30, 40, 50, 75, 100],
        pageSize: 5,
        totalItems: 0
      },
      backendServiceApi: {
        service: new GridOdataService(),
        options: {
          enableCount: this.isCountEnabled,
          version: 4
        },
        preProcess: () => this.displaySpinner(true),
        process: (query) => this.getData(query),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.displaySpinner(false);
          this.getCallback(response);
        }
      }
    };
    //#endregion

  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  getCallback(response) {

    this.gridOptions.pagination.totalItems = response['@odata.count'];

    if (this.metrics) {
      this.metrics.totalItemCount = response['@odata.count'];
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
    return this.service.getAll(`$expand=tipodeseguro,areaDeNegocio,grupoAgencia,atendente,agencia&${query}`);
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
        '',
        `Mapeamento ${retorno.adicionar ? 'adicionado' : 'atualizado'}!`,
        'success'
      );
    }
  }

  openPanel() {
    this.router.navigate(['cadastrar',
  ], { relativeTo: this.route});
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.itemSelecionado = null;
      this.display = false;
    }
  }


  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir o Mapeamento: ${item.dataContext.id}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.service.deletar(item.dataContext.id).subscribe(()=>{
          const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id
        )})
      }
    }).then((result) => {
      if (result.value) {
        const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
        this.angularGrid.gridService.deleteItemById(metadata.dataContext.id)
        Swal.fire(
          '',
          'O Mapeamento foi deletado.',
          'success'
        );
      }
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
  ], { relativeTo: this.route});
  }
}

